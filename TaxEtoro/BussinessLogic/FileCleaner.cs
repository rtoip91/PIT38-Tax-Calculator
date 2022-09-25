using Database.DataAccess.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TaxEtoro.Interfaces;

namespace TaxEtoro.BussinessLogic
{
    internal sealed class FileCleaner : IFileCleaner
    {
        private readonly string _filePath;
        private readonly IFileDataAccess _fileDataAccess;
        private readonly ILogger<FileCleaner> _logger;

        public FileCleaner(IConfiguration configuration,
            IFileDataAccess fileDataAccess,
            ILogger<FileCleaner> logger)
        {
            _filePath = configuration.GetValue<string>("ResultStorageFolder");
            _fileDataAccess = fileDataAccess;
            _logger = logger;
        }

        public async Task CleanCalculationResultFiles()
        {
            var fileNames = await _fileDataAccess.GetCalculationResultFilesToDelete();
            if(!fileNames.Any())
            {
                _logger.LogInformation("No result files to be deleted");
            }

            foreach (var fileName in fileNames)
            {
                string path = $"{_filePath}\\{fileName}";
                FileInfo fileInfo = new FileInfo(path);

                if(!fileInfo.Exists)
                {
                    await _fileDataAccess.SetAsDeleted(fileName);
                    _logger.LogInformation($"File:{fileName} is already deleted");
                    return;
                }

                try
                {
                    fileInfo.Delete();
                    await _fileDataAccess.SetAsDeleted(fileName);
                    _logger.LogInformation($"File:{fileName} was deleted");
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, $"Deletion of file {fileName} failed.");
                }               

            }
        }
    }
}
