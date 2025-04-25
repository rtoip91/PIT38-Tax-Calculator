using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TaxCalculatingService.Interfaces;

namespace TaxCalculatingService.BussinessLogic;

internal sealed class FileProcessingService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public FileProcessingService(IServiceScopeFactory scopeFactory)
    {
       _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();
                var fileProcessor = scope.ServiceProvider.GetRequiredService<IFileProcessor>();
                await fileProcessor.ProcessFiles(stoppingToken);
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }
        catch (TaskCanceledException)
        {
        }
    }
}