using Database.Enums;

namespace ExcelReader.Dto;

public class CurrentVersionData : ICurrentVersionData
{
    public FileVersion FileVersion { get; set; }
}