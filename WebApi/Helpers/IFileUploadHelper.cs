namespace WebApi.Helpers
{
    public interface IFileUploadHelper
    {
        Task<Guid?> UploadFile(IFormFile inputExcelFile);
    }
}
