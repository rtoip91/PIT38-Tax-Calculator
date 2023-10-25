using System.IO;
using System.Threading.Tasks;
using ExcelReader.Dto;

namespace ExcelReader.Interfaces
{
    public interface IExcelFileHandler
    {
        Task<ExtractedDataDto> ExtractDataFromExcel(MemoryStream fileContent);
    }
}
