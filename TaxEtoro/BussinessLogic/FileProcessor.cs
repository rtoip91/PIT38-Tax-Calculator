using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Threading;
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
using TaxCalculatingService.BussinessLogic;
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
        private readonly object _lock;
        private readonly SemaphoreSlim _semaphore;


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
            _semaphore = new SemaphoreSlim(0);
            _lock = new object();
        }


        private async Task ProcessSingleFile(Guid operation)
        {
            var directory = FileInputUtil.GetDirectory(@_configuration.GetValue<string>("InputFileStorageFolder"));
            var filename = await _fileDataAccess.GetInputFileNameAsync(operation);
            var file = directory.GetFiles(filename).FirstOrDefault();
            if (file is null)
            {
                _logger.LogWarning($"File {filename} was not found, marking as deleted.");
                await _fileDataAccess.SetAsDeletedAsync(operation);
                return;
            }

            await ProcessFile(directory, file, operation).ContinueWith( _ => RemoveFile(file));

        }

        public async Task ProcessFiles()
        {
            await ReduceSemaphore();

            IList<Guid> operations = await _fileDataAccess.GetOperationsToProcessAsync();

            if (!operations.Any())
            {
                _logger.LogInformation("No pending operations detected waiting for new operation");
                await _semaphore.WaitAsync();
                await ProcessFiles();
                return;
            }

            Stopwatch stopwatch = Stopwatch.StartNew();

            do
            {
                await Parallel.ForEachAsync(operations, async (operation,token) =>
                {
                    await ProcessSingleFile(operation);
                });

                operations = await _fileDataAccess.GetOperationsToProcessAsync();

            } while (operations.Any());

            stopwatch.Stop();
            var stopwatchResult = stopwatch.Elapsed;

            _logger.LogInformation($"Calculation took {stopwatchResult:m\\:ss\\.fff}");

            _exchangeRatesLocker.ClearLockers();
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            GC.Collect();

            await ProcessFiles();
        }

        private async Task ReduceSemaphore()
        {
            if (_semaphore.CurrentCount == 1)
            {
                await _semaphore.WaitAsync();
            }
        }

        private void RemoveFile(FileInfo file)
        {
            file.Delete();
            _logger.LogInformation($"File: {file.Name} was deleted");
        }

        private Task ProcessFile(DirectoryInfo directory, FileInfo file, Guid operation)
        {
            var task = Task.Run(async () =>
            {
                try
                {
                    await _fileDataAccess.SetAsInProgressAsync(operation);
                    await using AsyncServiceScope scope = _serviceProvider.CreateAsyncScope();
                    var dto = await Calculate(directory, file, scope, operation);
                    await PresentCalculationResults(dto, file, scope, operation);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"Error during processing file {file.FullName}");
                }
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
            await _fileDataAccess.SetAsCalculatedAsync(operation, dtoString);
            return dto;
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
            throw error;
        }

        public void OnNext(FileUploadedEvent value)
        {
            lock (_lock)
            {
                if (_semaphore.CurrentCount == 0)
                {
                    _semaphore.Release();
                }
            }
        }
    }
}