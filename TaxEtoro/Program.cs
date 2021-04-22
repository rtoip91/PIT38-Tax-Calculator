﻿using System.Threading.Tasks;
using Autofac;
using ExcelReader;
using ExcelReader.Interfaces;
using TaxEtoro.BussinessLogic;

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
            ICalculator calculator = scope.Resolve<ICalculator>();

            await reader.ImportDataFromExcelIntoDbAsync();
            await calculator.Calculate();
        }

        private static void RegisterContainer()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<ExcelDataExtractor>().As<IExcelDataExtractor>();
            builder.RegisterType<CfdCalculator>().As<ICfdCalculator>();
            builder.RegisterType<Calculator>().As<ICalculator>();
            Container = builder.Build();
        }
    }
}
