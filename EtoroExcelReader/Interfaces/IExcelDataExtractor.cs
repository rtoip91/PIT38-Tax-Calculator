using System.IO;
using System.Threading.Tasks;

namespace ExcelReader.Interfaces
{
    /// <summary>
    /// Interface for extracting data from an Excel file.
    /// </summary>
    public interface IExcelDataExtractor
    {
        /// <summary>
        /// Imports data from an Excel file into the application.
        /// </summary>
        /// <param name="fileContent">The content of the Excel file as a MemoryStream.</param>
        /// <returns>A Task that represents the asynchronous operation. The Task's result is true if the data was imported successfully; otherwise, false.</returns>
        public Task<bool> ImportDataFromExcel(MemoryStream fileContent);
    }
}