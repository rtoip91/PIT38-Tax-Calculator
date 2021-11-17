using Calculations.Dto;

namespace Calculations.Interfaces
{
    public interface ICalculationsFacade
    {
        public Task<CalculationResultDto> CalculateTaxes();
    }
}
