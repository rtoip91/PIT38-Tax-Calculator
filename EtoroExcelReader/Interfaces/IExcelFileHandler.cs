using System.IO;
using System.Threading.Tasks;
using ExcelReader.Dto;

namespace ExcelReader.Interfaces
{
    /// <summary>
    /// Interface for handling Excel files.
    /// </summary>
    public interface IExcelFileHandler
    {
        /// <summary>
        /// Extracts data from an Excel file.
        /// </summary>
        /// <param name="fileContent">The content of the Excel file as a MemoryStream.</param>
        /// <returns>A Task that represents the asynchronous operation. The Task's result is an ExtractedDataDto object that contains the extracted data.</returns>
        Task<ExtractedDataDto> ExtractDataFromExcel(MemoryStream fileContent);
    }
}