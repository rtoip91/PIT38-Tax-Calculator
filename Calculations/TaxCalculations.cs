using Calculations.Dto;
using Calculations.Interfaces;

namespace Calculations;

public class TaxCalculations : ITaxCalculations
{
    private readonly ICalculator<CalculationResultDto> _calculator;
    private readonly ICalculationEvents _events;

    public TaxCalculations( ICalculator<CalculationResultDto> calculator,
        ICalculationEvents events)
    {
        _calculator = calculator;
        _events = events;
    }

    public async Task<CalculationResultDto> CalculateTaxes()
    {
        var result = await _calculator.Calculate<CalculationResultDto>();
        return result;
    }
}