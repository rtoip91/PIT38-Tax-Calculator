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
        private bool _isDisposed;


        public ActionPerformer(IExcelDataExtractor reader,
            ITaxCalculations taxCalculations,
            IDataCleaner dataCleaner,
            IFileWriter fileWriter)
        {
            _reader = reader;
            _taxCalculations = taxCalculations;
            _dataCleaner = dataCleaner;
            _fileWriter = fileWriter;
            _isDisposed = false;
        }

        public async ValueTask DisposeAsync()
        {
            if (!_isDisposed)
            {
                Console.WriteLine("Czyszczenie bazy danych");
                await _dataCleaner.CleanData();
                _isDisposed = true;               
            }
        }

        public void OnAppClose(object sender, EventArgs e)
        {
            _ = DisposeAsync();
        }

        public async Task<CalculationResultDto> PerformCalculations()
        {
            try
            {
                await _reader.ImportDataFromExcelIntoDbAsync();
                var result = await _taxCalculations.CalculateTaxes();
                PresentRessults(result);
                return result;
            }
            catch (Exception e)
            {
                throw;
            }           
        }

        public async Task PresentCalcucaltionResults(CalculationResultDto result)
        {
            await _fileWriter.PresentData(result);
        }

        private void PresentRessults(CalculationResultDto result)
        {
            Console.WriteLine();
            Console.WriteLine("CFD:");
            Console.WriteLine($"Zysk = {result.CdfDto.Gain} PLN");
            Console.WriteLine($"Strata = {result.CdfDto.Loss} PLN");
            Console.WriteLine($"Dochód = {result.CdfDto.Income} PLN");
            Console.WriteLine();

            Console.WriteLine("Kryptowaluty:");
            Console.WriteLine($"Koszt zakupu = {result.CryptoDto.Cost} PLN");
            Console.WriteLine($"Przychód = {result.CryptoDto.Revenue} PLN");
            Console.WriteLine($"Dochód = {result.CryptoDto.Income} PLN");
            Console.WriteLine($"Niesprzedane kryptowaluty = {result.CryptoDto.UnsoldCryptos} PLN");
            Console.WriteLine();

            Console.WriteLine("Dywidendy:");
            Console.WriteLine($"Suma dywidend = {result.DividendDto.Dividend} PLN");
            Console.WriteLine();

            Console.WriteLine("Akcje:");
            Console.WriteLine($"Koszt zakupu = {result.StockDto.Cost} PLN");
            Console.WriteLine($"Przychód = {result.StockDto.Revenue} PLN");
            Console.WriteLine($"Dochód = {result.StockDto.Income} PLN");
            Console.WriteLine();
        }
    }
}