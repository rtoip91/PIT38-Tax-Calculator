using System.Collections.Generic;
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
            StockCalculations = new List<StockEntity>();
            PurchasedCryptoCalculations = new List<PurchasedCryptoEntity>();
            SoldCryptoCalculations = new List<SoldCryptoEntity>();
        }

        public IList<IncomeByCountryEntity> IncomeByCountryEntities { get;}
        public IList<ClosedPositionEntity> ClosedPositions { get;} 
        public IList<TransactionReportEntity> TransactionReports { get;}
        public IList<DividendEntity> Dividends { get;}
        public IList<StockEntity> StockCalculations { get; }
        public IList<PurchasedCryptoEntity> PurchasedCryptoCalculations { get; }
        public IList<SoldCryptoEntity> SoldCryptoCalculations { get; }

    }
}
