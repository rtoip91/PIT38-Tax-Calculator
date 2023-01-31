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
        if (_currentFileVersion == FileVersion.V2021A || _currentFileVersion == FileVersion.V2021)
        {
            return new V2021Converter();
        }

        return null;
    }
}