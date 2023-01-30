using ExcelReader.Converters;
using ExcelReader.Interfaces;

namespace ExcelReader.Factory;

internal sealed class ConverterFactory : IConverterFactory
{
    public IRowToEntityConverter GetConverter()
    {
        return new V2021Converter();
    }
}