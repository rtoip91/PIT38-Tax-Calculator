using AutoMapper;
using Database.Entities;
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
