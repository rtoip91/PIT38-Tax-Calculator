using TaxEtoro.Interfaces;

namespace WebApi.Extensions
{
    internal static class WebApplicationExtension
    {
        public static void RunPeriodicTasks(this WebApplication? webApplication)
        {
            var actionPerformer = webApplication?.Services.GetService<IActionPerformer>();
            if (actionPerformer == null)
            {
                return;
            }

            Task.Factory.StartNew(() => actionPerformer.PerformCalculationsAndWriteResultsPeriodically(), TaskCreationOptions.LongRunning);
            Task.Factory.StartNew(() => actionPerformer.ClearResultFilesPeriodically(), TaskCreationOptions.LongRunning);


            
        }

    }
}
