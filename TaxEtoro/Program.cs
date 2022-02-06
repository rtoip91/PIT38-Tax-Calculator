using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TaxEtoro.Interfaces;
using TaxEtoro.Statics;

namespace TaxEtoro
{
    class Program
    {
        private static IServiceProvider Services { get; set; }

        static Program()
        {
            Services = ServiceRegistration.Register();
        }

        static async Task Main(string[] args)
        {
            var timer = new Stopwatch();
            timer.Start();
            await using var scope = Services.CreateAsyncScope();
            var actionPerformer = scope.ServiceProvider.GetService<IActionPerformer>();
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(actionPerformer.OnAppClose);
            await actionPerformer.PerformCalculations();
            timer.Stop();
            TimeSpan timeTaken = timer.Elapsed;
            Console.WriteLine( $"Time taken: {timeTaken:m\\:ss\\.fff}");
            Console.WriteLine("Wciśnij dowolny klawisz aby zaprezentowac wyniki");
            _=Console.ReadKey();
            await actionPerformer.PresentCalcucaltionResults();
        }
    }
}