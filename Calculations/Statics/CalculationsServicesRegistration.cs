using Calculations.Calculators;
using Calculations.Dto;
using Calculations.Interfaces;
using Database.DataAccess;
using Database.DataAccess.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Calculations.Statics;

public static class CalculationsServicesRegistration
{
    public static void RegisterServices(IServiceCollection services)
    {
        services.AddTransient<IExchangeRates, ExchangeRates>();
        services.AddScoped<ICalculator<CalculationResultDto>, Calculator>();
        services.AddScoped<ICalculationEvents>(x => (Calculator)x.GetService<ICalculator<CalculationResultDto>>());
        services.AddTransient<ICalculator<CfdCalculatorDto>, CfdCalculator>();
        services.AddTransient<ICalculator<CryptoDto>, CryptoCalculator>();
        services.AddTransient<ICalculator<DividendCalculatorDto>, DividendCalculator>();
        services.AddTransient<ICalculator<StockCalculatorDto>, StockCalculator>();
        services.AddScoped<IExchangeRatesDataAccess, ExchangeRatesDataAccess>();
        services.AddScoped<ICfdEntityDataAccess, CfdEntityDataAccess>();
        services.AddScoped<IPurchasedCryptoEntityDataAccess, PurchasedCryptoEntityDataAccess>();
        services.AddScoped<ISoldCryptoEntityDataAccess, SoldCryptoEntityDataAccess>();
        services.AddScoped<IStockEntityDataAccess, StockEntityDataAccess>();
        services.AddScoped<IClosedPositionsDataAccess, ClosedPositionsDataAccess>();
        services.AddScoped<ITransactionReportsDataAccess, TransactionReportsDataAccess>();
        services.AddScoped<IDividendsDataAccess, DividendsDataAccess>();
        services.AddScoped<IDividendCalculationsDataAccess, DividendCalculationsDataAccess>();
        services.AddMemoryCache();

        services.AddHttpClient("ExchangeRates", config =>
        {
            config.BaseAddress = new Uri($"http://api.nbp.pl/api/exchangerates/rates/a/");
            config.Timeout = new TimeSpan(0, 0, 10);
            config.DefaultRequestHeaders.Clear();
        });
    }
}