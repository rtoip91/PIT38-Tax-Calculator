using Calculations.Dto;
using Calculations.Interfaces;
using Database.DataAccess.Interfaces;
using ExcelReader.Interfaces;
using System;
using System.Threading.Tasks;
using ResultsPresenter.Interfaces;
using TaxEtoro.Interfaces;

namespace TaxEtoro.BussinessLogic
{
    internal class ActionPerformer : IActionPerformer
    {
        private IExcelDataExtractor _reader;
        private ITaxCalculations _taxCalculations;
        private IDataCleaner _dataCleaner;
        private IFileWriter _fileWriter;
        private readonly IFileDataAccess _fileDataAccess;
        private bool _isDisposed;


        public ActionPerformer(IExcelDataExtractor reader,
            ITaxCalculations taxCalculations,
            IDataCleaner dataCleaner,
            IFileWriter fileWriter,
            IFileDataAccess fileDataAccess)
        {
            _reader = reader;
            _taxCalculations = taxCalculations;
            _dataCleaner = dataCleaner;
            _fileWriter = fileWriter;
            _isDisposed = false;
            _fileDataAccess = fileDataAccess;
        }

        public async ValueTask DisposeAsync()
        {
            if (!_isDisposed)
            {
                await _dataCleaner.CleanData();
                _isDisposed = true;               
            }
        }

        public void OnAppClose(object sender, EventArgs e)
        {
            _ = DisposeAsync();
        }

        public async Task<CalculationResultDto> PerformCalculations(string directory, string fileName)
        {
            _fileDataAccess.SetFileName(fileName);
            Console.WriteLine($"Rozpoczęto przetwarzanie pliku: {_fileDataAccess.GetFileName()}");
            await _reader.ImportDataFromExcel(directory,fileName);
            var result = await _taxCalculations.CalculateTaxes();
            PresentRessults(result);
            return result;
        }

        public async Task PresentCalcucaltionResults(CalculationResultDto result)
        {
            await _fileWriter.PresentData(result);
        }

        private void PresentRessults(CalculationResultDto result)
        {
            Console.WriteLine($"Zakończono przetwarzanie pliku: {_fileDataAccess.GetFileName()}");
        }
    }
}