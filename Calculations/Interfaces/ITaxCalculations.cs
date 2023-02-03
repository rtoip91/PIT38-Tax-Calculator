using Calculations.Dto;

namespace Calculations.Interfaces
{
    public interface ITaxCalculations
    {
        public Task<CalculationResultDto?> CalculateTaxes();
    }
}