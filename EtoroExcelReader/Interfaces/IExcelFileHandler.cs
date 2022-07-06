using System.Threading.Tasks;
using ExcelReader.Dto;

namespace ExcelReader.Interfaces
{
    public interface IExcelFileHandler
    {
        Task<ExtractedDataDto> ExtractDataFromExcel(string directory, string fileName);
    }
}
