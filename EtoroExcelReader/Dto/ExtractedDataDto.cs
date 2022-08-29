using System.Collections.Generic;
using EtoroExcelReader.Dto;

namespace ExcelReader.Dto
{
    public record ExtractedDataDto
    {
        public ExtractedDataDto()
        {
            ClosedPositions = new List<ClosedPositionExcelDto>();
            TransactionReports = new List<TransactionReportExcelDto>();
            Dividends = new List<DividendDto>();
        }

        public IList<ClosedPositionExcelDto> ClosedPositions { get;}
        public IList<TransactionReportExcelDto> TransactionReports { get; }
        public IList<DividendDto> Dividends { get; }
    }
}
