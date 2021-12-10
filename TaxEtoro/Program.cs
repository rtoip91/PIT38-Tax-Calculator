using System;
using System.IO;
using System.Threading.Tasks;
using Autofac;
using Calculations;
using Calculations.Interfaces;
using ExcelReader;
using ExcelReader.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using TaxEtoro.BussinessLogic;
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
            var builder = new ConfigurationBuilder();
            BuildConfig(builder);
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .Enrich.FromLogContext()
                .WriteTo.File("log.txt", fileSizeLimitBytes: 300000, rollOnFileSizeLimit: true)
                .CreateLogger();

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {

                })
                .UseSerilog()
                .Build();

            await using ILifetimeScope scope = Container.BeginLifetimeScope();
            IActionPerformer actionPerformer = scope.Resolve<IActionPerformer>();
            await actionPerformer.PerformCalculations();           
        }

        private static void BuildConfig(IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIROMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables();
        }

        private static void RegisterContainer()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<ExcelDataExtractor>().As<IExcelDataExtractor>();                     
            builder.RegisterType<DataCleaner>().As<IDataCleaner>();           
            builder.RegisterType<EventsSubscriber>().As<IEventsSubscriber>();
            builder.RegisterType<CalculationsFacade>().As<ICalculationsFacade>();
            builder.RegisterType<ActionPerformer>().As<IActionPerformer>();
            Container = builder.Build();
        }
    }
}
