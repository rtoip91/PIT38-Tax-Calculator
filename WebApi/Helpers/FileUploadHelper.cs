using Database.DataAccess.Interfaces;
using Database.Enums;
using ExcelReader.Validators;
using TaxCalculatingService.BussinessLogic;
using WebApi.Controllers;

namespace WebApi.Helpers
{
    public sealed class FileUploadHelper : IFileUploadHelper
    {
        private readonly IConfiguration _configuration;
        private readonly IFileDataAccess _fileDataAccess;
        private readonly ILogger<UploadFileController> _logger;
        private readonly IExcelStreamValidator _excelStreamValidator;

        public FileUploadHelper(IConfiguration configuration,
            IFileDataAccess fileDataAccess,
            ILogger<UploadFileController> logger,
            IExcelStreamValidator excelStreamValidator)
        {
            _configuration = configuration;
            _fileDataAccess = fileDataAccess;
            _logger = logger;
            _excelStreamValidator = excelStreamValidator;
        }

        private readonly HashSet<SubscriptionToken> _subscriptions = new();

        public IDisposable Subscribe(IObserver<FileUploadedEvent> observer)
        {
            var subscription = new SubscriptionToken(this, observer);
            _subscriptions.Add(subscription);
            return subscription;
        }

        public async Task<Guid?> UploadFile(IFormFile inputExcelFile)
        {
            long size = inputExcelFile.Length;

            if (inputExcelFile.Length > 0)
            {
                var fileVersion = await _excelStreamValidator.ValidateFileVersion(inputExcelFile.OpenReadStream());
                if (fileVersion == FileVersion.Unsupported)
                {
                    _logger.LogWarning("Wrong file provided");
                    return null;
                }

                var guid = Guid.NewGuid();
                string filename = await _fileDataAccess.AddNewFileAsync(guid);

                var filePath = Path.Combine(_configuration["InputFileStorageFolder"],
                    filename);

                await using (var stream = File.Create(filePath))
                {
                    await inputExcelFile.CopyToAsync(stream);
                }

                _logger.LogInformation($"Successfully uploaded a file {filename}");


                foreach (var sub in _subscriptions)
                {
                    sub.Observer.OnNext(new FileUploadedEvent
                    {
                        OperationGuid = guid
                    });
                }

                return guid;
            }

            _logger.LogWarning("Wrong file provided");
            return null;
        }

        private sealed class SubscriptionToken : IDisposable
        {
            private readonly FileUploadHelper _fileUploadHelper;
            public readonly IObserver<FileUploadedEvent> Observer;
            public SubscriptionToken(FileUploadHelper fileUploadHelper, IObserver<FileUploadedEvent> observer)
            {
                this._fileUploadHelper = fileUploadHelper;
                Observer = observer;
            }
            public void Dispose()
            {
                _fileUploadHelper._subscriptions.Remove(this);
            }
        }
    }
}
