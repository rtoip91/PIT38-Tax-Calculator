using System.Collections.Generic;

namespace ExcelReader.Dictionaries.Interfaces;

internal interface IExcelData
{
    public IReadOnlyDictionary<int,string> SpreadSheets { get;}
    public IReadOnlyDictionary<string,IList<string>> ColumnNames { get; }
}