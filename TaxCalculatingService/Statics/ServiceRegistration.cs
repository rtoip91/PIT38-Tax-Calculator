using Calculations;
using Calculations.Interfaces;
using Database.DataAccess;
using Database.DataAccess.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using ResultsPresenter;
using ResultsPresenter.Interfaces;
using TaxCalculatingService.BussinessLogic;
using TaxCalculatingService.Interfaces;

namespace TaxCalculatingService.Statics;

public static class TaxEtoroServiceRegistration
{
    public static void RegisterServices(IServiceCollection services)
    {       
        services.AddTransient<ITaxCalculations, TaxCalculations>();
        services.AddTransient<IClosedPositionsDataAccess, ClosedPositionsDataAccess>();
        services.AddTransient<ITransactionReportsDataAccess, TransactionReportsDataAccess>();
        services.AddTransient<IFileWriter, FileWriter>();
        services.AddTransient<IFileProcessor, FileProcessor>();
        services.AddHostedService<FileCleaningService>();
        services.AddHostedService<FileProcessingService>();
    }
}