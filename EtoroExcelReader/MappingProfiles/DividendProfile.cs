using AutoMapper;
using Database.Entities.InMemory;
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