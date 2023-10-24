using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Database.DataAccess.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TaxCalculatingService.BussinessLogic
{
    internal sealed class FileCleaningService : BackgroundService
    {
        private readonly string _filePath;
        private readonly IFileDataAccess _fileDataAccess;
        private readonly ILogger<FileCleaningService> _logger;
        
        private readonly PeriodicTimer _fileCleanTimer;

        public FileCleaningService(IConfiguration configuration,
            IFileDataAccess fileDataAccess,
            ILogger<FileCleaningService> logger)
        {
            _filePath = configuration.GetValue<string>("ResultStorageFolder");
            _fileDataAccess = fileDataAccess;
            _logger = logger;
            _fileCleanTimer = new PeriodicTimer(TimeSpan.FromHours(1));
        }

        public async Task CleanCalculationResultFiles()
        {
            _logger.LogInformation("Triggering the calculation result file cleaning");
           
            var numberOfOldData = await _fileDataAccess.RemoveOldDataAboutDeletedFiles();
            _logger.LogInformation("Removed {NumberOfOldData} data about old deleted files from the database",
                numberOfOldData);
            
            var fileNames = await _fileDataAccess.GetCalculationResultFilesToDeleteAsync();
            if (!fileNames.Any())
            {
                _logger.LogInformation("No result files to be deleted");
                return;
            }

            await Parallel.ForEachAsync(fileNames, async (filename, _) => await DeleteFile(filename));
        }

        private async Task DeleteFile(string fileName)
        {
            string path = $"{_filePath}\\{fileName}";
            FileInfo fileInfo = new FileInfo(path);

            if (!fileInfo.Exists)
            {
                await _fileDataAccess.SetAsDeletedAsync(fileName);
                _logger.LogInformation("Result file:{FileName} is already deleted", fileName);
                return;
            }

            try
            {
                fileInfo.Delete();
                await _fileDataAccess.SetAsDeletedAsync(fileName);
                _logger.LogInformation("Result file:{FileName} was deleted", fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Deletion of result file {FileName} failed", fileName);
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _fileCleanTimer.WaitForNextTickAsync(stoppingToken);
                await CleanCalculationResultFiles();
            }
        }
    }
}