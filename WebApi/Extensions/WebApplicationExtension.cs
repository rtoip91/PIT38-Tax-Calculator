using TaxEtoro.Interfaces;

namespace WebApi.Extensions
{
    internal static class WebApplicationExtension
    {

        private static Task calculationTask;
        private static Task clearResultTask;

        public static void RunPeriodicTasks(this WebApplication? webApplication)
        {
            var actionPerformer = webApplication?.Services.GetService<IActionPerformer>();
            if (actionPerformer == null)
            {
                return;
            }

            var fileProcessor = webApplication?.Services.GetService<IFileProcessor>();

            calculationTask = Task.Factory.StartNew(() => fileProcessor.ProcessFiles(), TaskCreationOptions.LongRunning);
            clearResultTask= Task.Factory.StartNew(() => actionPerformer.ClearResultFilesPeriodically(), TaskCreationOptions.LongRunning);


            
        }

    }
}
