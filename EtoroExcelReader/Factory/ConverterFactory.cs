using System;
using Database.Enums;
using ExcelReader.Converters;
using ExcelReader.Dto;

namespace ExcelReader.Factory;

internal sealed class ConverterFactory : IConverterFactory
{
    private readonly FileVersion _currentFileVersion;

    public ConverterFactory(ICurrentVersionData currentVersionData)
    {
        _currentFileVersion = currentVersionData.FileVersion;
    }

    public IRowToEntityConverter GetConverter()
    {
        return _currentFileVersion switch
        {
            FileVersion.V2021 => new V2021Converter(),
            FileVersion.V2022 => new V2022Converter(),
            FileVersion.V2021A => new V2021Converter(),
            FileVersion.V2023 => new V2023Converter(),
            _ => throw new ArgumentException("Unsupported version")
        };
    }
}