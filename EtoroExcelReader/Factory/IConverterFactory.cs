using ExcelReader.Converters;

namespace ExcelReader.Factory;

internal interface IConverterFactory
{
    IRowToEntityConverter GetConverter();
}