using System.Collections.Generic;
using Database.Enums;
using ExcelReader.Dictionaries.Interfaces;
using ExcelReader.Dictionaries.V2021;
using ExcelReader.Dictionaries.V2022;
using ExcelReader.Dictionaries.V2023;
using ExcelReader.Dictionaries.V2025;

namespace ExcelReader.Dictionaries;

internal class VersionData : IVersionData
{
    public IReadOnlyDictionary<FileVersion, IExcelData> Versions { get; }

    public VersionData()
    {
        Versions = new Dictionary<FileVersion, IExcelData>
        {
            { FileVersion.V2021, new V2021ExcelData() },
            { FileVersion.V2021A, new V2021AExcelData() },
            { FileVersion.V2022, new V2022ExcelData() },
            { FileVersion.V2023, new V2023ExcelData() },
            { FileVersion.V2025, new V2025ExcelData() }
        };
    }
}