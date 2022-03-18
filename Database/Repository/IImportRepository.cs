using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Entities;

namespace Database.Repository
{
    public interface IImportRepository
    {
        IList<IncomeByCountryEntity> IncomeByCountryEntities { get;}
        IList<ClosedPositionEntity> ClosedPositions { get; }
        IList<TransactionReportEntity> TransactionReports { get; }
    }
}
