using Database.DataAccess.Interfaces;
using Database.Enums;
using ExcelReader.Validators;

namespace WebApi.Helpers
{
    public sealed class FileUploadHelper : IFileUploadHelper
    {
        private readonly IConfiguration _configuration;
        private readonly IFileDataAccess _fileDataAccess;
        private readonly ILogger<FileUploadHelper> _logger;
        private readonly IExcelStreamValidator _excelStreamValidator;

        public FileUploadHelper(IConfiguration configuration,
            IFileDataAccess fileDataAccess,
            ILogger<FileUploadHelper> logger,
            IExcelStreamValidator excelStreamValidator)
        {
            _configuration = configuration;
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

           

                //var filePath = Path.Combine(_configuration["InputFileStorageFolder"],filename);
                
                

                await using (var stream = new MemoryStream())
                {
                    await inputExcelFile.CopyToAsync(stream);
                    var guid = Guid.NewGuid();
                    string filename = await _fileDataAccess.AddNewFileAsync(guid,fileVersion, stream);
                    _logger.LogInformation("Successfully uploaded a file {Filename}", filename);
                    return guid;
                }

               
            }

            _logger.LogWarning("Wrong file provided");
            return null;
        }
    }
}
