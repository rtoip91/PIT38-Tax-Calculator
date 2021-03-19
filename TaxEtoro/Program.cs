using EtoroExcelReader;
using System;
using System.Threading.Tasks;

namespace TaxEtoro
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ExcelReader reader = new ExcelReader();
            await reader.Run();
        }
    }
}
