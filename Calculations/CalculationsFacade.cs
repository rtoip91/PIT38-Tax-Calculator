using Autofac;
using Calculations.Calculators;
using Calculations.Dto;
using Calculations.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Calculations
{
    public class CalculationsFacade : ICalculationsFacade
    {
        private IServiceProvider _services;
        private IEventsSubscriber _eventsSubscriber;


        public CalculationsFacade(IEventsSubscriber eventsSubscriber)
        {
            _services = RegisterContainer();
            _eventsSubscriber = eventsSubscriber;
        }

        public async Task<CalculationResultDto> CalculateTaxes()
        {
            await using var scope = _services.CreateAsyncScope();            
            ICalculator<CalculationResultDto> calculator = scope.ServiceProvider.GetService<ICalculator<CalculationResultDto>>();
            ICalculationEvents events = scope.ServiceProvider.GetService<ICalculationEvents>();

            if (events != null)
            {
                events.CfdCalculationFinished += _eventsSubscriber.AfterCfd;
                events.DividendCalculationFinished += _eventsSubscriber.AfterDividend;
                events.CryptoCalculationFinished += _eventsSubscriber.AfterCrypto;
                events.StockCalculationFinished += _eventsSubscriber.AfterStock;
            }

            var result = await calculator.Calculate<CalculationResultDto>();
            return result;
        }


        private IServiceProvider RegisterContainer()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddTransient<IExchangeRatesGetter, ExchangeRatesGetter>();
                    services.AddScoped<ICalculator<CalculationResultDto>, Calculator>();
                    services.AddScoped<ICalculationEvents>(x => (Calculator) x.GetService<ICalculator<CalculationResultDto>>());
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
