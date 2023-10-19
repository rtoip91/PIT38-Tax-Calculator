using Database.DataAccess.Interfaces;
using Database.Enums;
using ExcelReader.Validators;

namespace WebApi.Helpers
{
    public sealed class FileUploadHelper : IFileUploadHelper
    {
        private readonly IFileDataAccess _fileDataAccess;
        private readonly ILogger<FileUploadHelper> _logger;
        private readonly IExcelStreamValidator _excelStreamValidator;

        public FileUploadHelper(
            IFileDataAccess fileDataAccess,
            ILogger<FileUploadHelper> logger,
            IExcelStreamValidator excelStreamValidator)
        {
            _fileDataAccess = fileDataAccess;
            _logger = logger;
            _excelStreamValidator = excelStreamValidator;
        }

        public async Task<Guid?> UploadFile(IFormFile inputExcelFile)
        {
            if (inputExcelFile.Length > 0)
            {
                var fileVersion = await _excelStreamValidator.ValidateFileVersion(inputExcelFile.OpenReadStream());
                if (fileVersion == FileVersion.Unsupported)
                {
                    _logger.LogWarning("Wrong file provided");
                    return null;
                }

                await using var stream = new MemoryStream();
                await inputExcelFile.CopyToAsync(stream);
                var guid = Guid.NewGuid();
                string filename = await _fileDataAccess.AddNewFileAsync(guid, fileVersion, stream);
                _logger.LogInformation("Successfully uploaded a file {Filename}", filename);
                return guid;
            }

            _logger.LogWarning("Wrong file provided");
            return null;
        }
    }
}