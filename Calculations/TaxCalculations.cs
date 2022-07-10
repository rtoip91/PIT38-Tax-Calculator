using Calculations.Dto;
using Calculations.Interfaces;

namespace Calculations;

public class TaxCalculations : ITaxCalculations
{
    private readonly ICalculator<CalculationResultDto> _calculator;  

    public TaxCalculations( ICalculator<CalculationResultDto> calculator)
    {
        _calculator = calculator;       
    }

    public async Task<CalculationResultDto> CalculateTaxes()
    {
        var result = await _calculator.Calculate<CalculationResultDto>();
        return result;
    }
}