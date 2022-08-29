using AutoMapper;
using Database.Entities.InMemory;
using EtoroExcelReader.Dto;

namespace ExcelReader.MappingProfiles
{
    internal class TransactionReportProfile : Profile
    {
        public TransactionReportProfile()
        {
            CreateMap<TransactionReportExcelDto, TransactionReportEntity>();
        }
    }
}