using System.IO;
using System.Threading.Tasks;

namespace ExcelReader.Interfaces
{
    public interface IExcelDataExtractor
    {
        public Task<bool> ImportDataFromExcel(MemoryStream fileContent);
    }
}