using System;
using System.IO;
using System.Threading.Tasks;
using Database.Enums;
using ExcelReader.Dictionaries.Interfaces;
using OfficeOpenXml;

namespace ExcelReader.Validators;

public sealed class ExcelStreamValidator : IExcelStreamValidator
{
    private readonly IVersionData _versionData;

    public ExcelStreamValidator(IVersionData versionData)
    {
        _versionData = versionData;
    }

    public async Task<FileVersion> ValidateFileVersion(Stream excelFileStream)
    {
        try
        {
            ExcelPackage.License.SetNonCommercialPersonal("Rtoip91"); 
            using ExcelPackage package = new ExcelPackage();
            await package.LoadAsync(excelFileStream);

            foreach (var versionData in _versionData.Versions)
            {
                FileVersion currentVersion = versionData.Key;
                IExcelData excelData = versionData.Value;
                bool result = true;

                if (package.Workbook.Worksheets.Count != excelData.SpreadSheets.Count)
                {
                    continue;
                }

                for (int i = 0; i < package.Workbook.Worksheets.Count; i++)
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[i];
                    string currentWorksheetName = worksheet.Name.Trim();
                    string expectedWorksheetName = excelData.SpreadSheets[i];

                    if (currentWorksheetName != expectedWorksheetName)
                    {
                        result = false;
                        break;
                    }

                    var columns = excelData.ColumnNames[expectedWorksheetName];
                    if (columns == null && excelData.ColumnNames[expectedWorksheetName] == null)
                    {
                        continue;
                    }

                    for (int j = 1; j <= columns.Count; j++)
                    {
                        var currentColumnName = worksheet.Cells[1, j].Value.ToString()?.Trim();
                        var expectedColumnName = excelData.ColumnNames[expectedWorksheetName][j - 1];

                        if (currentColumnName != expectedColumnName)
                        {
                            result = false;
                            break;
                        }
                    }

                    if (!result)
                    {
                        break;
                    }
                }

                if (result)
                {
                    return currentVersion;
                }
            }

            return FileVersion.Unsupported;
        }
        catch (Exception)
        {
            return FileVersion.Unsupported;
        }
    }
}