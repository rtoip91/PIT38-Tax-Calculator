using System;
using System.Threading;
using System.Threading.Tasks;
using TaxEtoro.Interfaces;

namespace TaxEtoro.BussinessLogic
{
    internal sealed class ActionPerformer : IActionPerformer
    {       
        private readonly IFileCleaner _fileCleaner;
        private readonly IFileProcessor _fileProcessor;

        private readonly PeriodicTimer _calculationsTimer;
        private readonly PeriodicTimer _fileCleanTimer;


        public ActionPerformer(IFileCleaner fileCleaner,
            IFileProcessor fileProcessor)
        {
            _calculationsTimer = new PeriodicTimer(TimeSpan.FromMinutes(1));
            _fileCleanTimer = new PeriodicTimer(TimeSpan.FromMinutes(1));
            _fileCleaner = fileCleaner;
            _fileProcessor = fileProcessor;
        }      

        public async Task PerformCalculationsAndWriteResultsPeriodically()
        {
            while (true)
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