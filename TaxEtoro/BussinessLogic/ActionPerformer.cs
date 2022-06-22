using Calculations.Dto;
using Calculations.Interfaces;
using Database.DataAccess.Interfaces;
using ExcelReader.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExcelReader;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ResultsPresenter.Interfaces;
using TaxEtoro.Interfaces;

namespace TaxEtoro.BussinessLogic
{
    internal class ActionPerformer : IActionPerformer
    {
        private readonly IDataCleaner _dataCleaner;
        private readonly IConfiguration _configuration;
        private readonly PeriodicTimer _periodicTimer;
        private readonly IServiceProvider _serviceProvider;
        private bool _isDisposed;

        public ActionPerformer(IDataCleaner dataCleaner,
            IConfiguration configuration,
            IServiceProvider serviceProvider)
        {
            _dataCleaner = dataCleaner;
            _isDisposed = false;
            _configuration = configuration;
            _periodicTimer = new PeriodicTimer(TimeSpan.FromMinutes(1));
            _serviceProvider = serviceProvider;
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
            var directory = FileInputUtil.GetDirectory(@_configuration.GetValue<string>("InputFileStorageFolder"));
            var files = directory.GetFiles("*xlsx");

            IList<Task> tasks = new List<Task>();

            if (!files.Any())
            {
                Console.WriteLine("No files detected");
                return;
            }

            foreach (var filename in files)
            {
                var task = Task.Run(async () =>
                {
                    await using AsyncServiceScope scope = serviceProvider.CreateAsyncScope();
                    var dto = await PerformCalculations(directory.FullName, filename.Name, scope);
                    await PresentCalculationResults(dto, scope);
                });

                var fileRemoval = task.ContinueWith(_ =>
                {
                    filename.Delete();
                    Console.WriteLine($"File: {filename.Name} was deleted");
                });

                tasks.Add(task);
                tasks.Add(fileRemoval);
            }

            await Task.WhenAll(tasks);
        }

        public async Task PerformCalculationsAndWriteResultsPeriodically()
        {
            while (await _periodicTimer.WaitForNextTickAsync())
            {
                await DoWork(_serviceProvider);
            }
        }

        private async Task<CalculationResultDto> PerformCalculations(string directory, string fileName,
            AsyncServiceScope scope)
        {
            IExcelDataExtractor reader = scope.ServiceProvider.GetService<IExcelDataExtractor>();
            ITaxCalculations taxCalculations = scope.ServiceProvider.GetService<ITaxCalculations>();
            IFileDataAccess fileDataAccess = scope.ServiceProvider.GetService<IFileDataAccess>();

            fileDataAccess.SetFileName(fileName);
            Console.WriteLine($"Rozpoczęto przetwarzanie pliku: {fileDataAccess.GetFileName()}");
            await reader.ImportDataFromExcel(directory, fileName);
            var result = await taxCalculations.CalculateTaxes();

            return result;
        }

        private async Task PresentCalculationResults(CalculationResultDto result, AsyncServiceScope scope)
        {
            IFileDataAccess fileDataAccess = scope.ServiceProvider.GetService<IFileDataAccess>();
            IFileWriter fileWriter = scope.ServiceProvider.GetService<IFileWriter>();

            await fileWriter.PresentData(result);
            Console.WriteLine($"Zakończono przetwarzanie pliku: {fileDataAccess.GetFileName()}");
        }
    }
}