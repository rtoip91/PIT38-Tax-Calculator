using System.Collections.Generic;
using Database.Entities.InMemory;

namespace ExcelReader.Dto
{
    public record ExtractedDataDto()
    {
        public IList<ClosedPositionEntity> ClosedPositions { get;} = new List<ClosedPositionEntity>();
        public IList<TransactionReportEntity> TransactionReports { get; } = new List<TransactionReportEntity>();
        public IList<DividendEntity> Dividends { get; } = new List<DividendEntity>();
    }
}
