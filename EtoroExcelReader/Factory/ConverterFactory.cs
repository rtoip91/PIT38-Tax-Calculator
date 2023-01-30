using ExcelReader.Converters;
using ExcelReader.Interfaces;

namespace ExcelReader.Factory;

internal class ConverterFactory : IConverterFactory
{
    public IRowToEntityConverter GetConverter()
    {
        return new V2021Converter();
    }
}