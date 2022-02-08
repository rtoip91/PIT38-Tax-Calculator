
using Calculations.Dto;

namespace ResultPresenter.Interfaces
{
    public interface IFileWriter
    { 
        Task PresentData(CalculationResultDto calculationResultDto);
    }
}
