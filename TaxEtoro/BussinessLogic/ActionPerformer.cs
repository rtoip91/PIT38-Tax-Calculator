using Calculations.Dto;
using Calculations.Interfaces;
using Database.DataAccess.Interfaces;
using ExcelReader.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ResultsPresenter.Interfaces;
using TaxEtoro.Interfaces;
using Microsoft.Extensions.Logging;
using ExcelReader.Statics;

namespace TaxEtoro.BussinessLogic
{
    internal class ActionPerformer : IActionPerformer
    {
        private readonly IDataCleaner _dataCleaner;
        private readonly IConfiguration _configuration;       
        private readonly IServiceProvider _serviceProvider;
        private readonly IFileDataAccess _fileDataAccess;
        private readonly ILogger<ActionPerformer> _logger;
        private readonly IFileCleaner _fileCleaner;

        private readonly PeriodicTimer _calculationsTimer;
        private readonly PeriodicTimer _fileCleanTimer;

        private bool _isDisposed;

        public ActionPerformer(IDataCleaner dataCleaner,
            IConfiguration configuration,
            IServiceProvider serviceProvider,
            IFileDataAccess fileDataAccess,
            ILogger<ActionPerformer> logger,
            IFileCleaner fileCleaner)
        {
            _dataCleaner = dataCleaner;
            _isDisposed = false;
            _configuration = configuration;
            _calculationsTimer = new PeriodicTimer(TimeSpan.FromMinutes(1));
            _fileCleanTimer = new PeriodicTimer(TimeSpan.FromSeconds(10));
            _serviceProvider = serviceProvider;
            _fileDataAccess = fileDataAccess;
            _logger = logger;
            _fileCleaner = fileCleaner;
        }

        public async ValueTask DisposeAsync()
        {
            if (!_isDisposed)
            {
                await _dataCleaner.CleanData();
                _isDisposed = true;
            }
        }


        private async Task DoWork(IServiceProvider serviceProvider)
        {
            IList<Task> tasks = new List<Task>();

            var directory = FileInputUtil.GetDirectory(@_configuration.GetValue<string>("InputFileStorageFolder"));
            var operations = await _fileDataAccess.GetOperationsToProcess();

            if (!operations.Any())
            {
                _logger.LogInformation("No pending operations detected");
                return;
            }

            foreach (var operation in operations)
            {
                var filename = await _fileDataAccess.GetInputFileName(operation);
                var file = directory.GetFiles(filename).FirstOrDefault();
                if (file is null)
                {
                    continue;
                }

                var task = Task.Run(async () =>
                {
                    await using AsyncServiceScope scope = serviceProvider.CreateAsyncScope();
                    var dto = await Calculate(directory, file, scope, operation);
                    await PresentCalculationResults(dto, operation, scope);
                });

                var fileRemoval = task.ContinueWith(_ =>
                {
                    file.Delete();
                    _logger.LogInformation($"File: {file.Name} was deleted");
                });

                tasks.Add(task);
                tasks.Add(fileRemoval);
            }

            await Task.WhenAll(tasks);
        }

        private async Task<CalculationResultDto> Calculate(DirectoryInfo directory, FileInfo file,
            AsyncServiceScope scope, Guid operation)
        {
            var dto = await PerformCalculations(directory.FullName, file.Name, scope);
            var dtoString = JsonConvert.SerializeObject(dto);
            await _fileDataAccess.SetAsCalculated(operation, dtoString);
            return dto;
        }

        public async Task PerformCalculationsAndWriteResultsPeriodically()
        {
            while (await _calculationsTimer.WaitForNextTickAsync())
            {
                await DoWork(_serviceProvider);
            }
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

        private async Task PresentCalculationResults(CalculationResultDto result, Guid operationGuid,
            AsyncServiceScope scope)
        {
            IFileWriter fileWriter = scope.ServiceProvider.GetService<IFileWriter>();

            string fileName = await fileWriter.PresentData(operationGuid, result);

            _logger.LogInformation($"Created results in {fileName}");
        }

        public async Task ClearResultFilesPeriodically()
        {
            while (await _fileCleanTimer.WaitForNextTickAsync())
            {
                await _fileCleaner.CleanCalculationResultFiles();
            }
        }
    }
}