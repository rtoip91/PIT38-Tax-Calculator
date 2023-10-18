using System;
using System.Collections.Generic;
using System.Runtime;
using Database.Entities.InMemory;

namespace Database.Repository
{
    internal sealed class DataRepository : IDataRepository
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
            DividendsCalculations = new List<DividendCalculationsEntity>();
            CfdCalculations = new List<CfdEntity>();
        }

        public IList<IncomeByCountryEntity> IncomeByCountryEntities { get; }
        public IList<ClosedPositionEntity> ClosedPositions { get; }
        public IList<TransactionReportEntity> TransactionReports { get; }
        public IList<DividendEntity> Dividends { get; }
        public IList<StockEntity> StockCalculations { get; }
        public IList<PurchasedCryptoEntity> PurchasedCryptoCalculations { get; }
        public IList<SoldCryptoEntity> SoldCryptoCalculations { get; }
        public IList<DividendCalculationsEntity> DividendsCalculations { get; }
        public IList<CfdEntity> CfdCalculations { get; }
        public Guid OperationGuid { get; set; }
    }
}