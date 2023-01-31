using System.Collections.Generic;
using Database.Enums;

namespace ExcelReader.Dictionaries.Interfaces;

public interface IVersionData
{
    IReadOnlyDictionary<FileVersion,IExcelData> Versions { get; }
}