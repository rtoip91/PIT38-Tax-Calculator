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
                })
                .Build();
            return host.Services;
        }
    }
}
