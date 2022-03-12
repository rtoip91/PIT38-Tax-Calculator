using AutoMapper;
using Database.Entities;
using ExcelReader.Dto;

namespace ExcelReader.MappingProfiles
{
    internal class DividendProfile : Profile
    {
        public DividendProfile()
        {
            CreateMap<DividendDto, DividendEntity>();
        }
    }
}
