using System.Collections.Generic;
using Database.Entities;

namespace Database.Repository
{
    public interface IDataRepository
    {
        IList<IncomeByCountryEntity> IncomeByCountryEntities { get;}
        IList<ClosedPositionEntity> ClosedPositions { get; }
        IList<TransactionReportEntity> TransactionReports { get; }
        public IList<DividendEntity> Dividends { get; }
    }
}
