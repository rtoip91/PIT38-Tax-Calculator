using AutoMapper;
using Database.Entities.InMemory;
using EtoroExcelReader.Dto;

namespace ExcelReader.MappingProfiles
{
    internal class ClosedPositionProfile : Profile
    {
        public ClosedPositionProfile()
        {
            CreateMap<ClosedPositionExcelDto, ClosedPositionEntity>();
        }
    }
}