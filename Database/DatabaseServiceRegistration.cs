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
            using ( var dbContext = new ApplicationDbContext())
            {
                dbContext.MigrateDatabase();
            }
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
            services.AddTransient<IFileDataAccess, FileDataAccess>();
            services.AddTransient<ICryptocurrencyDataAccess, CryptocurrencyDataAccess>();
            services.AddScoped<IDataRepository, DataRepository>();
        }
    }
}