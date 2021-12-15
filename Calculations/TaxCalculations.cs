using Calculations.Dto;
using Calculations.Interfaces;
using Calculations.Statics;
using Microsoft.Extensions.DependencyInjection;

namespace Calculations
{
    public class TaxCalculations : ITaxCalculations
    {
        private IServiceProvider _services;
        private IEventsSubscriber _eventsSubscriber;


        public TaxCalculations(IEventsSubscriber eventsSubscriber)
        {
            _services = ServicesRegistrator.Services();
            _eventsSubscriber = eventsSubscriber;
        }

        public async Task<CalculationResultDto> CalculateTaxes()
        {
            await using var scope = _services.CreateAsyncScope();
            ICalculator<CalculationResultDto> calculator =
                scope.ServiceProvider.GetService<ICalculator<CalculationResultDto>>();
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
    }
}