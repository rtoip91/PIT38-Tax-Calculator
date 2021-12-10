using System;
using System.IO;
using System.Threading.Tasks;
using Autofac;
using Calculations;
using Calculations.Interfaces;
using ExcelReader;
using ExcelReader.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using TaxEtoro.BussinessLogic;
using TaxEtoro.Interfaces;

namespace TaxEtoro
{
    class Program
    {
        private static IServiceProvider Services   { get; set; }

        static Program()
        {
            RegisterServices();
        }
        
        static async Task Main(string[] args)
        {
            await using var scope = Services.CreateAsyncScope();
            var actionPerformer = scope.ServiceProvider.GetService<IActionPerformer>();            
            await actionPerformer.PerformCalculations();            
        }

        private static void BuildConfig(IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIROMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables();
        }

        private static void RegisterServices()
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
                    services.AddTransient<IExcelDataExtractor, ExcelDataExtractor>();
                    services.AddTransient<IDataCleaner, DataCleaner>();
                    services.AddTransient<IEventsSubscriber, EventsSubscriber>();
                    services.AddTransient<ICalculationsFacade, CalculationsFacade>();
                    services.AddTransient<IActionPerformer, ActionPerformer>();
                })
                .UseSerilog()
                .Build();

            Services = host.Services;
        }
    }
}
