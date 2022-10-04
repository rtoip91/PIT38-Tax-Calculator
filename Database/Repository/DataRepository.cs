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

        public IList<IncomeByCountryEntity> IncomeByCountryEntities { get; private set; }
        public IList<ClosedPositionEntity> ClosedPositions { get; private set; }
        public IList<TransactionReportEntity> TransactionReports { get; private set; }
        public IList<DividendEntity> Dividends { get; private set; }
        public IList<StockEntity> StockCalculations { get; private set; }
        public IList<PurchasedCryptoEntity> PurchasedCryptoCalculations { get; private set; }
        public IList<SoldCryptoEntity> SoldCryptoCalculations { get; private set; }
        public IList<DividendCalculationsEntity> DividendsCalculations { get; private set; }
        public IList<CfdEntity> CfdCalculations { get; private set; }
        public Guid OperationGuid { get; set; }

        public void Dispose()
        {
            IncomeByCountryEntities = null;
            ClosedPositions = null;
            TransactionReports = null;
            Dividends = null;
            StockCalculations = null;
            PurchasedCryptoCalculations = null;
            SoldCryptoCalculations = null;
            DividendsCalculations = null;
            CfdCalculations = null;           
        }
    }
}