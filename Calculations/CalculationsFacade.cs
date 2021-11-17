using Autofac;
using Calculations.Calculators;
using Calculations.Dto;
using Calculations.Interfaces;

namespace Calculations
{
    public class CalculationsFacade : ICalculationsFacade
    {
        private IContainer _container;
        private IEventsSubscriber _eventsSubscriber;


        public CalculationsFacade(IEventsSubscriber eventsSubscriber)
        {
            _container = RegisterContainer();
            _eventsSubscriber = eventsSubscriber;
        }

        public async Task<CalculationResultDto> CalculateTaxes()
        {
            await using ILifetimeScope scope = _container.BeginLifetimeScope();
            ICalculator<CalculationResultDto> calculator = scope.Resolve<ICalculator<CalculationResultDto>>();
            ICalculationEvents events = scope.Resolve<ICalculationEvents>();

            events.CfdCalculationFinished += _eventsSubscriber.AfterCfd;
            events.DividendCalculationFinished += _eventsSubscriber.AfterDividend;
            events.CryptoCalculationFinished += _eventsSubscriber.AfterCrypto;
            events.StockCalculationFinished += _eventsSubscriber.AfterStock;

            var result = await calculator.Calculate<CalculationResultDto>();
            return result;
        }


        private IContainer RegisterContainer()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<ExchangeRatesGetter>().As<IExchangeRatesGetter>();
            builder.RegisterType<Calculator>().As<ICalculator<CalculationResultDto>, ICalculationEvents>().InstancePerLifetimeScope();
            builder.RegisterType<CfdCalculator>().As<ICalculator<CfdCalculatorDto>>();
            builder.RegisterType<CryptoCalculator>().As<ICalculator<CryptoDto>>();
            builder.RegisterType<DividendCalculator>().As<ICalculator<DividendCalculatorDto>>();
            builder.RegisterType<StockCalculator>().As<ICalculator<StockCalculatorDto>>();                
            return builder.Build();
        }
    }
}
