using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using ExcelReader;
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
            var directory = FileInputUtil.GetDirectory(@"d:\\TestFile");
            var files = directory.GetFiles("*xlsx");

            IList<Task> tasks = new List<Task>();

            var timer = new Stopwatch();
            timer.Start();

            foreach (var filename in files)
            {
                var task = Task.Run(async () => { await DoWork(filename, directory.FullName); }).ContinueWith((t) =>
                {
                    if (t.IsFaulted)
                    {
                        throw t.Exception;
                    }
                });

                tasks.Add(task);
            }

            await Task.WhenAll(tasks);

            timer.Stop();
            TimeSpan timeTaken = timer.Elapsed;
            Console.WriteLine($"Time taken: {timeTaken:m\\:ss\\.fff}\n");
        }

        private static async Task DoWork(FileInfo filename, string directory)
        {
            await using var scope = Services.CreateAsyncScope();
            var actionPerformer = scope.ServiceProvider.GetService<IActionPerformer>();
            AppDomain.CurrentDomain.ProcessExit += actionPerformer.OnAppClose;
            var result = await actionPerformer.PerformCalculations(directory, filename.Name);
            await actionPerformer.PresentCalcucaltionResults(result);
        }
    }
}