using Database.DataAccess;
using Database.DataAccess.Interfaces;
using Database.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Database
{
    public static class DatabaseServiceRegistration
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<IExchangeRatesDataAccess, ExchangeRatesDataAccess>();
            services.AddTransient<ICfdEntityDataAccess, CfdEntityDataAccess>();
            services.AddTransient<IPurchasedCryptoEntityDataAccess, PurchasedCryptoEntityDataAccess>();
            services.AddTransient<ISoldCryptoEntityDataAccess, SoldCryptoEntityDataAccess>();
            services.AddTransient<IStockEntityDataAccess, StockEntityDataAccess>();
            services.AddTransient<IClosedPositionsDataAccess, ClosedPositionsDataAccess>();
            services.AddTransient<ITransactionReportsDataAccess, TransactionReportsDataAccess>();
            services.AddTransient<IDividendsDataAccess, DividendsDataAccess>();
            services.AddTransient<IDividendCalculationsDataAccess, DividendCalculationsDataAccess>();
            services.AddTransient<IIncomeByCountryDataAccess, IncomeByCountryDataAccess>();
            services.AddScoped<IFileDataAccess, FileDataAccess>();
            services.AddScoped<IDataRepository, DataRepository>();
        }
    }
}