using Database.DataAccess.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaxEtoro.Interfaces;

namespace TaxEtoro.BussinessLogic
{
    internal class ActionPerformer : IActionPerformer
    {       
        private readonly IFileCleaner _fileCleaner;
        private readonly IFileProcessor _fileProcessor;

        private readonly PeriodicTimer _calculationsTimer;
        private readonly PeriodicTimer _fileCleanTimer;

        private bool _isDisposed;

        public ActionPerformer(IFileCleaner fileCleaner,
            IFileProcessor fileProcessor)
        {            
            _isDisposed = false;
            _calculationsTimer = new PeriodicTimer(TimeSpan.FromMinutes(1));
            _fileCleanTimer = new PeriodicTimer(TimeSpan.FromMinutes(10));
            _fileCleaner = fileCleaner;
            _fileProcessor = fileProcessor;
        }      

        public async Task PerformCalculationsAndWriteResultsPeriodically()
        {
            while (await _calculationsTimer.WaitForNextTickAsync())
            {
                await _fileProcessor.ProcessFiles();
            }
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