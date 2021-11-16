using System;
using System.Threading.Tasks;
using Autofac;
using ExcelReader;
using ExcelReader.Interfaces;
using TaxEtoro.BussinessLogic;
using TaxEtoro.BussinessLogic.Dto;
using TaxEtoro.Interfaces;

namespace TaxEtoro
{
    class Program
    {
        private static IContainer Container { get; set; }

        static Program()
        {
            RegisterContainer();
        }
        
        static async Task Main(string[] args)
        {
            await using ILifetimeScope scope = Container.BeginLifetimeScope();
            IExcelDataExtractor reader = scope.Resolve<IExcelDataExtractor>();
            ICalculator<CalculationResultDto> calculator = scope.Resolve<ICalculator<CalculationResultDto>>();
            ICalculationEvents events =  scope.Resolve<ICalculationEvents>();
            IDataCleaner dataCleaner = scope.Resolve<IDataCleaner>();   

            try
            {
                await reader.ImportDataFromExcelIntoDbAsync();
                var result = await calculator.Calculate<CalculationResultDto>();

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
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Console.WriteLine("Czyszczenie bazy danych");
                await dataCleaner.CleanData();
            }
        }

        private static void RegisterContainer()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<ExcelDataExtractor>().As<IExcelDataExtractor>();                  
            builder.RegisterType<ExchangeRatesGetter>().As<IExchangeRatesGetter>();           
            builder.RegisterType<DataCleaner>().As<IDataCleaner>();
            builder.RegisterType<Calculator>().As<ICalculator<CalculationResultDto>, ICalculationEvents>().InstancePerLifetimeScope();
            builder.RegisterType<CfdCalculator>().As<ICalculator<CfdCalculatorDto>>();
            builder.RegisterType<CryptoCalculator>().As<ICalculator<CryptoDto>>();
            builder.RegisterType<DividendCalculator>().As<ICalculator<DividendCalculatorDto>>();
            builder.RegisterType<StockCalculator>().As<ICalculator<StockCalculatorDto>>();
            Container = builder.Build();
        }
    }
}
