using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using TaxCalculatingService.Interfaces;

namespace TaxCalculatingService.BussinessLogic;

internal sealed class FileProcessingService : BackgroundService
{
    private readonly IFileProcessor _fileProcessor;

    public FileProcessingService(IFileProcessor fileProcessor)
    {
        _fileProcessor = fileProcessor;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _fileProcessor.ProcessFiles(stoppingToken);
    }
}