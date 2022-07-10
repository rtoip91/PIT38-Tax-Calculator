using Calculations.Calculators;
using Calculations.Dto;
using Calculations.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Calculations.Statics;

public static class CalculationsServicesRegistration
{
    public static void RegisterServices(IServiceCollection services)
    {
        services.AddTransient<IExchangeRates, ExchangeRates>();
        services.AddSingleton<IExchangeRatesLocker, ExchangeRatesLocker>();
        services.AddScoped<ICalculator<CalculationResultDto>, Calculator>();       
        services.AddTransient<ICalculator<CfdCalculatorDto>, CfdCalculator>();
        services.AddTransient<ICalculator<CryptoDto>, CryptoCalculator>();
        services.AddTransient<ICalculator<DividendCalculatorDto>, DividendCalculator>();
        services.AddTransient<ICalculator<StockCalculatorDto>, StockCalculator>();
        services.AddMemoryCache();

        services.AddHttpClient("ExchangeRates", config =>
        {
            config.BaseAddress = new Uri($"http://api.nbp.pl/api/exchangerates/rates/a/");
            config.Timeout = new TimeSpan(0, 0, 10);
            config.DefaultRequestHeaders.Clear();
        });
    }
}