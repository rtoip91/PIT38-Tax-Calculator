using System;
using System.Threading;
using System.Threading.Tasks;
using TaxCalculatingService.Interfaces;
using TaxEtoro.Interfaces;

namespace TaxCalculatingService.BussinessLogic
{
    internal sealed class ActionPerformer : IActionPerformer
    {       
        private readonly IFileCleaner _fileCleaner;
        private readonly IFileProcessor _fileProcessor;

        private readonly PeriodicTimer _fileCleanTimer;


        public ActionPerformer(IFileCleaner fileCleaner,
            IFileProcessor fileProcessor)
        {
            _fileCleanTimer = new PeriodicTimer(TimeSpan.FromMinutes(1));
            _fileCleaner = fileCleaner;
            _fileProcessor = fileProcessor;
        }      

      

        public async Task ClearResultFilesPeriodically()
        {
            while (await _fileCleanTimer.WaitForNextTickAsync())
            {
                await _fileCleaner.CleanCalculationResultFiles();
            }
        }
    }
}