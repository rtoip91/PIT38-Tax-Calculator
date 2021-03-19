using AutoMapper;
using Database.Entities;
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
