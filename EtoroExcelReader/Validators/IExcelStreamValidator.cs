using System.IO;
using System.Threading.Tasks;
using Database.Enums;

namespace ExcelReader.Validators;

public interface IExcelStreamValidator
{
    Task<FileVersion> ValidateFileVersion(Stream excelFileStream);
}