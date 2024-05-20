using System.IO;
using System.Threading.Tasks;
using Database.Enums;

namespace ExcelReader.Validators
{
    /// <summary>
    /// Interface for validating the version of an Excel file.
    /// </summary>
    public interface IExcelStreamValidator
    {
        /// <summary>
        /// Validates the version of an Excel file.
        /// </summary>
        /// <param name="excelFileStream">The stream of the Excel file to validate.</param>
        /// <returns>A Task that represents the asynchronous operation. The Task's result is the version of the Excel file.</returns>
        Task<FileVersion> ValidateFileVersion(Stream excelFileStream);
    }
}