using System;
using System.Collections.Generic;
using Database.Entities.InMemory;

namespace Database.Repository
{
    internal sealed class DataRepository : IDataRepository
    {
        public IList<IncomeByCountryEntity> IncomeByCountryEntities { get; } = new List<IncomeByCountryEntity>();
        public IList<ClosedPositionEntity> ClosedPositions { get; } = new List<ClosedPositionEntity>();
        public IList<TransactionReportEntity> TransactionReports { get; } = new List<TransactionReportEntity>();
        public IList<DividendEntity> Dividends { get; } = new List<DividendEntity>();
        public IList<StockEntity> StockCalculations { get; } = new List<StockEntity>();
        public IList<PurchasedCryptoEntity> PurchasedCryptoCalculations { get; } = new List<PurchasedCryptoEntity>();
        public IList<SoldCryptoEntity> SoldCryptoCalculations { get; } = new List<SoldCryptoEntity>();
        public IList<DividendCalculationsEntity> DividendsCalculations { get; } = new List<DividendCalculationsEntity>();
        public IList<CfdEntity> CfdCalculations { get; } = new List<CfdEntity>();
    }
}