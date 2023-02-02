using Database.Enums;

namespace ExcelReader.Dto;

public interface ICurrentVersionData
{
    FileVersion FileVersion { get; set; }
}