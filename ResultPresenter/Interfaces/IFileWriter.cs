using Calculations.Dto;

namespace ResultsPresenter.Interfaces
{
    public interface IFileWriter
    {
        Task PresentData(CalculationResultDto calculationResultDto);
    }
}