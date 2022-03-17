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
            services.AddScoped<IExchangeRatesDataAccess, ExchangeRatesDataAccess>();
            services.AddScoped<ICfdEntityDataAccess, CfdEntityDataAccess>();
            services.AddScoped<IPurchasedCryptoEntityDataAccess, PurchasedCryptoEntityDataAccess>();
            services.AddScoped<ISoldCryptoEntityDataAccess, SoldCryptoEntityDataAccess>();
            services.AddScoped<IStockEntityDataAccess, StockEntityDataAccess>();
            services.AddScoped<IClosedPositionsDataAccess, ClosedPositionsDataAccess>();
            services.AddScoped<ITransactionReportsDataAccess, TransactionReportsDataAccess>();
            services.AddScoped<IDividendsDataAccess, DividendsDataAccess>();
            services.AddScoped<IDividendCalculationsDataAccess, DividendCalculationsDataAccess>();
            services.AddScoped<IIncomeByCountryDataAccess, IncomeByCountryDataAccess>();
            services.AddScoped<IImportRepository, ImportRepository>();
        }
    }
}
