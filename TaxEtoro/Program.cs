using System.Threading.Tasks;
using Autofac;
using ExcelReader;
using ExcelReader.Interfaces;

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
            IExcelDataExtractor reader = scope.Resolve<IExcelDataExtractor>();
            await reader.ImportDataFromExcelIntoDbAsync();
        }

        private static void RegisterContainer()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<ExcelDataExtractor>().As<IExcelDataExtractor>();
            Container = builder.Build();
        }
    }
}
