using Calculations;
using Calculations.Interfaces;
using Database.DataAccess;
using Database.DataAccess.Interfaces;
using ExcelReader;
using ExcelReader.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;
using Calculations.Statics;
using Microsoft.Extensions.Logging;
using TaxEtoro.BussinessLogic;
using TaxEtoro.Interfaces;
using ResultPresenter.Interfaces;
using ResultsPresenter;

namespace TaxEtoro.Statics;

internal static class ServiceRegistration
{
    private static void BuildConfig(IConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile(
                $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIROMENT") ?? "Production"}.json",
                optional: true)
            .AddEnvironmentVariables();
    }

    public static IServiceProvider Register()
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
                RegisterServices(services);
                CalculationsServicesRegistration.RegisterServices(services);
            })
            .ConfigureLogging((context, logging) =>
            {
                logging.ClearProviders();
                logging.AddDebug();
            })
            .Build();

        return host.Services;
    }

    private static void RegisterServices(IServiceCollection services)
    {
        services.AddTransient<IExcelDataExtractor, ExcelDataExtractor>();
        services.AddTransient<IDataCleaner, DataCleaner>();
        services.AddTransient<IEventsSubscriber, EventsSubscriber>();
        services.AddTransient<ITaxCalculations, TaxCalculations>();
        services.AddTransient<IActionPerformer, ActionPerformer>();
        services.AddTransient<IClosedPositionsDataAccess, ClosedPositionsDataAccess>();
        services.AddTransient<ITransactionReportsDataAccess, TransactionReportsDataAccess>();
        services.AddTransient<IFileWriter, FileWriter>();
    }
}