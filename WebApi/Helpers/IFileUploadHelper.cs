using TaxCalculatingService.BussinessLogic;

namespace WebApi.Helpers
{
    public interface IFileUploadHelper : IObservable<FileUploadedEvent>
    {
        Task<Guid?> UploadFile(IFormFile inputExcelFile);
    }
}
