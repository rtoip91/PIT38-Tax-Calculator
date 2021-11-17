using System;
using System.Threading.Tasks;
using Autofac;
using Calculations;
using Calculations.Interfaces;
using ExcelReader;
using ExcelReader.Interfaces;
using TaxEtoro.BussinessLogic;
using TaxEtoro.Interfaces;

namespace TaxEtoro
{
    class Program
    {
        private static IContainer Container { get; set; }

        static Program()
        {
            RegisterContainer();
        }
        
        static async Task Main(string[] args)
        {
            await using ILifetimeScope scope = Container.BeginLifetimeScope();
            IActionPerformer actionPerformer = scope.Resolve<IActionPerformer>();
            await actionPerformer.PerformCalculations();           
        }      

        private static void RegisterContainer()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<ExcelDataExtractor>().As<IExcelDataExtractor>();                     
            builder.RegisterType<DataCleaner>().As<IDataCleaner>();           
            builder.RegisterType<EventsSubscriber>().As<IEventsSubscriber>();
            builder.RegisterType<CalculationsFacade>().As<ICalculationsFacade>();
            builder.RegisterType<ActionPerformer>().As<IActionPerformer>();
            Container = builder.Build();
        }
    }
}
