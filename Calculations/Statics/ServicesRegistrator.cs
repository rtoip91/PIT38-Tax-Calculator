using Calculations.Calculators;
using Calculations.Dto;
using Calculations.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Calculations.Statics
{
    internal static class ServicesRegistrator
    {
        public static IServiceProvider Services()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddTransient<IExchangeRatesGetter, ExchangeRatesGetter>();
                    services.AddScoped<ICalculator<CalculationResultDto>, Calculator>();
                    services.AddScoped<ICalculationEvents>(x => (Calculator)x.GetService<ICalculator<CalculationResultDto>>());
                    services.AddTransient<ICalculator<CfdCalculatorDto>, CfdCalculator>();
                    services.AddTransient<ICalculator<CryptoDto>, CryptoCalculator>();
                    services.AddTransient<ICalculator<DividendCalculatorDto>, DividendCalculator>();
                    services.AddTransient<ICalculator<StockCalculatorDto>, StockCalculator>();

                    services.AddHttpClient("ExchangeRates", config =>
                    {
                        config.BaseAddress = new Uri( $"http://api.nbp.pl/api/exchangerates/rates/a/");
                        config.Timeout = new TimeSpan(0, 0, 30);
                        config.DefaultRequestHeaders.Clear();
                    });
                })
                .Build();
            return host.Services;
        }
    }
}
