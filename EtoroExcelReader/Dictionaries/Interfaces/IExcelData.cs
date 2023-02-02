using System.Collections.Generic;

namespace ExcelReader.Dictionaries.Interfaces;

public interface IExcelData
{
    public IReadOnlyDictionary<int,string> SpreadSheets { get;}
    public IReadOnlyDictionary<string,IList<string>> ColumnNames { get; }
}