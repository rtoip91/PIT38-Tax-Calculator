using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Entities;

namespace Database.Repository
{
    internal class DataRepository : IDataRepository
    {
        public DataRepository()
        {
            IncomeByCountryEntities = new List<IncomeByCountryEntity>();
            ClosedPositions = new List<ClosedPositionEntity>();
            TransactionReports = new List<TransactionReportEntity>();
            Dividends = new List<DividendEntity>();
        }
        public IList<IncomeByCountryEntity> IncomeByCountryEntities { get;}
        public IList<ClosedPositionEntity> ClosedPositions { get;} 
        public IList<TransactionReportEntity> TransactionReports { get;}
        public IList<DividendEntity> Dividends { get;}

    }
}
