using Calculations;
using Calculations.Interfaces;
using Database.DataAccess;
using Database.DataAccess.Interfaces;
using ExcelReader;
using ExcelReader.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using TaxEtoro.BussinessLogic;
using TaxEtoro.Interfaces;
using ResultsPresenter;
using ResultsPresenter.Interfaces;

namespace TaxEtoro.Statics;

public static class TaxEtoroServiceRegistration
{
    public static void RegisterServices(IServiceCollection services)
    {
        services.AddTransient<IDataCleaner, DataCleaner>();
        services.AddTransient<ITaxCalculations, TaxCalculations>();
        services.AddTransient<IActionPerformer, ActionPerformer>();
        services.AddTransient<IClosedPositionsDataAccess, ClosedPositionsDataAccess>();
        services.AddTransient<ITransactionReportsDataAccess, TransactionReportsDataAccess>();
        services.AddTransient<IFileWriter, FileWriter>();
    }
}