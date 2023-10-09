namespace TaxCalculatingService.Interfaces;

public interface IFileProcessingControl
{
    void PauseProcessing();
    void ResumeProcessing();
}