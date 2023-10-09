using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using TaxCalculatingService.Interfaces;

namespace TaxCalculatingService.BussinessLogic;

internal sealed class FileProcessingService : BackgroundService
{
    private readonly IFileProcessor _fileProcessor;
    private readonly PeriodicTimer _periodicTimer;

    public FileProcessingService(IFileProcessor fileProcessor)
    {
        _fileProcessor = fileProcessor;
        _periodicTimer = new PeriodicTimer(TimeSpan.FromMinutes(1));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        
            while (!stoppingToken.IsCancellationRequested)
            {
                await _fileProcessor.ProcessFiles(stoppingToken);
                await _periodicTimer.WaitForNextTickAsync(stoppingToken);
            }

        }
        catch (TaskCanceledException)
        {
          
        }
    
    }
}