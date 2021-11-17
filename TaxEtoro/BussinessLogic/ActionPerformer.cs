using Calculations.Dto;
using Calculations.Interfaces;
using ExcelReader.Interfaces;
using System;
using System.Threading.Tasks;
using TaxEtoro.Interfaces;

namespace TaxEtoro.BussinessLogic
{
    internal class ActionPerformer : IActionPerformer
    {
        private IExcelDataExtractor _reader;
        private ICalculationsFacade _calculationsFacade;
        private IDataCleaner _dataCleaner;

        public ActionPerformer(IExcelDataExtractor reader, ICalculationsFacade calculationsFacade, IDataCleaner dataCleaner)
        {
            _reader = reader;
            _calculationsFacade = calculationsFacade;
            _dataCleaner = dataCleaner;
        }

        public async Task PerformCalculations()
        {
            try
            {
                await _reader.ImportDataFromExcelIntoDbAsync();
                var result = await _calculationsFacade.CalculateTaxes();
                PresentRessults(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Console.WriteLine("Czyszczenie bazy danych");
                await _dataCleaner.CleanData();
            }
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
