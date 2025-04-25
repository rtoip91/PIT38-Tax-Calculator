using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Database.DataAccess.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TaxCalculatingService.BussinessLogic
{
    internal sealed class FileCleaningService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<FileCleaningService> _logger;
        private readonly PeriodicTimer _fileCleanTimer;

        public FileCleaningService(
            IServiceScopeFactory scopeFactory,
            ILogger<FileCleaningService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _fileCleanTimer = new PeriodicTimer(TimeSpan.FromHours(1));
        }

        private async Task CleanCalculationResultFiles()
        {
            _logger.LogInformation("Triggering the calculation result file cleaning");

            using var scope = _scopeFactory.CreateScope();
            var fileDataAccess = scope.ServiceProvider.GetRequiredService<IFileDataAccess>();
            var numberOfOldData = await fileDataAccess.RemoveOldDataAboutDeletedFiles();
            _logger.LogInformation("Removed {NumberOfOldData} data about old deleted files from the database",
                numberOfOldData);

            var fileNames = await fileDataAccess.GetCalculationResultFilesToDeleteAsync();
            if (!fileNames.Any())
            {
                _logger.LogInformation("No result files to be deleted");
                return;
            }

            await Parallel.ForEachAsync(fileNames, async (filename, _) => await DeleteFile(filename));
        }

        private async Task DeleteFile(string fileName)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var fileDataAccess = scope.ServiceProvider.GetRequiredService<IFileDataAccess>();
                await fileDataAccess.SetAsDeletedAsync(fileName);
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