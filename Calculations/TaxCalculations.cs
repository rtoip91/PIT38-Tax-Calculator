using Calculations.Dto;
using Calculations.Interfaces;

namespace Calculations;

public class TaxCalculations : ITaxCalculations
{
    private readonly IEventsSubscriber _eventsSubscriber;
    private readonly ICalculator<CalculationResultDto> _calculator;
    private readonly ICalculationEvents _events;

    public TaxCalculations(IEventsSubscriber eventsSubscriber, ICalculator<CalculationResultDto> calculator,
        ICalculationEvents events)
    {
        _eventsSubscriber = eventsSubscriber;
        _calculator = calculator;
        _events = events;
    }

    public async Task<CalculationResultDto> CalculateTaxes()
    {
        _events.CfdCalculationFinished += _eventsSubscriber.AfterCfd;
        _events.DividendCalculationFinished += _eventsSubscriber.AfterDividend;
        _events.CryptoCalculationFinished += _eventsSubscriber.AfterCrypto;
        _events.StockCalculationFinished += _eventsSubscriber.AfterStock;

        var result = await _calculator.Calculate<CalculationResultDto>();
        return result;
    }
}