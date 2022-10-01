using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Threading.Tasks;
using Calculations.Dto;
using Calculations.Interfaces;
using Database.DataAccess.Interfaces;
using ExcelReader.Interfaces;
using ExcelReader.Statics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ResultsPresenter.Interfaces;
using TaxEtoro.Interfaces;

namespace TaxEtoro.BussinessLogic
{
    internal sealed class FileProcessor : IFileProcessor
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private readonly IFileDataAccess _fileDataAccess;
        private readonly ILogger<FileProcessor> _logger;
        private readonly IExchangeRatesLocker _exchangeRatesLocker;

        public FileProcessor(IServiceProvider serviceProvider,
            IConfiguration configuration,
            IFileDataAccess fileDataAccess,
            ILogger<FileProcessor> logger,
            IExchangeRatesLocker exchangeRatesLocker)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            _fileDataAccess = fileDataAccess;
            _logger = logger;
            _exchangeRatesLocker = exchangeRatesLocker;
        }

        public async Task ProcessFiles()
        {
            IList<Task> tasks = new List<Task>();

            var directory = FileInputUtil.GetDirectory(@_configuration.GetValue<string>("InputFileStorageFolder"));
            var operations = await _fileDataAccess.GetOperationsToProcess();


            if (!operations.Any())
            {
                _logger.LogInformation("No pending operations detected");
                return;
            }

            Stopwatch stopwatch = Stopwatch.StartNew();

            do
            {
                foreach (var operation in operations)
                {
                    var filename = await _fileDataAccess.GetInputFileName(operation);
                    var file = directory.GetFiles(filename).FirstOrDefault();
                    if (file is null)
                    {
                        continue;
                    }

                    var fileProcessingTask = ProcessFile(directory, file, operation);
                    var fileRemovalTask = RemoveFile(fileProcessingTask, file);

                    tasks.Add(fileProcessingTask);
                    tasks.Add(fileRemovalTask);
                }

                await Task.WhenAll(tasks);
                operations = await _fileDataAccess.GetOperationsToProcess();

            } while (operations.Any());

            stopwatch.Stop();
            var stopwatchResult = stopwatch.Elapsed;

            _logger.LogInformation($"Calculation took {stopwatchResult:m\\:ss\\.fff}");

            _exchangeRatesLocker.ClearLockers();
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            GC.Collect();

        }

        private Task RemoveFile(Task task, FileInfo file)
        {
            var fileRemoval = task.ContinueWith(_ =>
            {
                file.Delete();
                _logger.LogInformation($"File: {file.Name} was deleted");
            });
            return fileRemoval;
        }

        private Task ProcessFile(DirectoryInfo directory, FileInfo file, Guid operation)
        {
            var task = Task.Run(async () =>
            {
                await using AsyncServiceScope scope = _serviceProvider.CreateAsyncScope();
                var dto = await Calculate(directory, file, scope, operation);
                await PresentCalculationResults(dto, file, scope, operation);
            });
            return task;
        }

        private async Task<CalculationResultDto> PerformCalculations(string directory, string fileName,
            AsyncServiceScope scope)
        {
            IExcelDataExtractor reader = scope.ServiceProvider.GetService<IExcelDataExtractor>();
            ITaxCalculations taxCalculations = scope.ServiceProvider.GetService<ITaxCalculations>();

            _logger.LogInformation($"Started processing of the file {fileName}");
            await reader.ImportDataFromExcel(directory, fileName);
            var result = await taxCalculations.CalculateTaxes();

            return result;
        }

        private async Task PresentCalculationResults(CalculationResultDto result, FileInfo file,
            AsyncServiceScope scope, Guid operationGuid)
        {
            IFileWriter fileWriter = scope.ServiceProvider.GetService<IFileWriter>();

            string fileName = await fileWriter.PresentData(operationGuid, file, result);

            _logger.LogInformation($"Created results in {fileName}");
        }

        private async Task<CalculationResultDto> Calculate(DirectoryInfo directory, FileInfo file,
            AsyncServiceScope scope, Guid operation)
        {
            var dto = await PerformCalculations(directory.FullName, file.Name, scope);
            var dtoString = JsonConvert.SerializeObject(dto);
            await _fileDataAccess.SetAsCalculated(operation, dtoString);
            return dto;
        }
    }
}