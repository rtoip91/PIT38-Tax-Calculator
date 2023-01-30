using ExcelReader.Interfaces;

namespace ExcelReader.Factory;

internal interface IConverterFactory
{
    IRowToEntityConverter GetConverter();
}