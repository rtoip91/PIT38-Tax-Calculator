using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TaxEtoro.BussinessLogic;
using TaxEtoro.Interfaces;
using TaxEtoro.Statics;

namespace TaxEtoro
{
    class Program
    {
        private static readonly IActionPerformer ActionPerformer;

        static Program()
        {
            var serviceProvider = ServiceRegistration.ServiceProvider;
            ActionPerformer = serviceProvider.GetService<IActionPerformer>();
        }

        static async Task Main(string[] args)
        {
            var timer = new Stopwatch();

            timer.Start();

            await ActionPerformer.PerformCalculationsAndWriteResults();

            timer.Stop();

            TimeSpan timeTaken = timer.Elapsed;
            Console.WriteLine($"Time taken: {timeTaken:m\\:ss\\.fff}\n");
        }
    }
}