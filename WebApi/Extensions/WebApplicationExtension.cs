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

            actionPerformer.PerformCalculationsAndWriteResultsPeriodically();
            actionPerformer.ClearResultFilesPeriodically();
        }

    }
}
