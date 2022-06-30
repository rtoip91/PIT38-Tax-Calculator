using Calculations.Dto;

namespace ResultsPresenter.Interfaces
{
    public interface IFileWriter
    {
        Task PresentData(Guid operationId, CalculationResultDto calculationResultDto);
    }
}