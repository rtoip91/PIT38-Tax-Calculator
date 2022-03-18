using System;
using System.Collections.Generic;
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

            IList<string> fileNames = new List<string>
                { "TestFile2020.xlsx", "TestFile2021.xlsx", "TestFile2021v2.xlsx", "TestFile2022.xlsx" };

            foreach (var filename in fileNames)
            {
                var timer = new Stopwatch();
                timer.Start();

                await using var scope = Services.CreateAsyncScope();
                var actionPerformer = scope.ServiceProvider.GetService<IActionPerformer>();
                AppDomain.CurrentDomain.ProcessExit += actionPerformer.OnAppClose;
                var result = await actionPerformer.PerformCalculations(@"..\\TestFile", filename);

                timer.Stop();

                TimeSpan timeTaken = timer.Elapsed;
                Console.WriteLine($"Time taken: {timeTaken:m\\:ss\\.fff}\n");

                await actionPerformer.PresentCalcucaltionResults(result);
            }
            
        }
    }
}