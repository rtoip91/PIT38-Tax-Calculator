using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TaxEtoro.Interfaces;
using TaxEtoro.Statics;

namespace TaxEtoro
{
    class Program
    {
        private static IServiceProvider Services   { get; set; }

        static Program()
        {
            Services = ServiceRegistration.Register();
        }
        
        static async Task Main(string[] args)
        {
            await using var scope = Services.CreateAsyncScope();
            var actionPerformer = scope.ServiceProvider.GetService<IActionPerformer>();            
            await actionPerformer.PerformCalculations();            
        }       
    }
}
