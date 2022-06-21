using Calculations.Dto;
using Calculations.Interfaces;
using Database.DataAccess.Interfaces;
using ExcelReader.Interfaces;
using System;
using System.Collections.Generic;
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
        private readonly IServiceProvider _serviceProvider;
        private bool _isDisposed;
       


        public ActionPerformer(IDataCleaner dataCleaner,
            IConfiguration configuration,
            IServiceProvider serviceProvider)
        {
            _dataCleaner = dataCleaner;
            _isDisposed = false;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            AppDomain.CurrentDomain.ProcessExit += OnAppClose;
        }

        public async ValueTask DisposeAsync()
        {
            if (!_isDisposed)
            {
                await _dataCleaner.CleanData();
                _isDisposed = true;
            }
        }

        private void OnAppClose(object sender, EventArgs e)
        {
            _ = DisposeAsync();
        }

        public async Task PerformCalculationsAndWriteResults()
        {
            var directory = FileInputUtil.GetDirectory(@_configuration.GetValue<string>("InputFileStorageFolder"));
            var files = directory.GetFiles("*xlsx");

            IList<Task> tasks = new List<Task>();

            foreach (var filename in files)
            {
                var task = Task.Run(async () =>
                {
                    await using AsyncServiceScope scope = _serviceProvider.CreateAsyncScope();
                    var dto = await PerformCalculations(directory.FullName, filename.Name, scope);
                    await PresentCalcucaltionResults(dto, scope);
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

        private async Task PresentCalcucaltionResults(CalculationResultDto result, AsyncServiceScope scope)
        {
            IFileDataAccess fileDataAccess = scope.ServiceProvider.GetService<IFileDataAccess>();
            IFileWriter fileWriter = scope.ServiceProvider.GetService<IFileWriter>();

            await fileWriter.PresentData(result);
            Console.WriteLine($"Zakończono przetwarzanie pliku: {fileDataAccess.GetFileName()}");
        }
    }
}