using System.Collections.Generic;
using Database.Enums;
using ExcelReader.Dictionaries.Interfaces;
using ExcelReader.Dictionaries.V2021;

namespace ExcelReader.Dictionaries;

internal class VersionData : IVersionData
{
    public IReadOnlyDictionary<FileVersion, IExcelData> Versions { get; }

    public VersionData()
    {
        Versions = new Dictionary<FileVersion, IExcelData>
        {
            { FileVersion.V2021 , new V2021ExcelData()},
            { FileVersion.V2021A , new V2021aExcelData()}
        };
    }
}