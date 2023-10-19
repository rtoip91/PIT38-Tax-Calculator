using Calculations.Dto;

namespace ResultsPresenter.Interfaces
{
    public interface IFileWriter
    {
        Task<MemoryStream> PresentData(Guid operationId, MemoryStream inputFileContent,
            CalculationResultDto calculationResultDto);
    }
}