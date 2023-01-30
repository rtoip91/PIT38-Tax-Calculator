using System.Collections.Generic;
using Database.Enums;

namespace ExcelReader.Dictionaries.Interfaces;

internal interface IVersionData
{
    IReadOnlyDictionary<FileVersion,IExcelData> Versions { get; }
}