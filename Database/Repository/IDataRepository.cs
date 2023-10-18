using System;
using System.Collections.Generic;
using Database.Entities.InMemory;

namespace Database.Repository
{
    public interface IDataRepository
    {
        IList<IncomeByCountryEntity> IncomeByCountryEntities { get; }
        IList<ClosedPositionEntity> ClosedPositions { get; }
        IList<TransactionReportEntity> TransactionReports { get; }
        IList<DividendEntity> Dividends { get; }
        IList<StockEntity> StockCalculations { get; }
        IList<PurchasedCryptoEntity> PurchasedCryptoCalculations { get; }
        IList<SoldCryptoEntity> SoldCryptoCalculations { get; }
        IList<DividendCalculationsEntity> DividendsCalculations { get; }
        IList<CfdEntity> CfdCalculations { get; }
        Guid OperationGuid { get; set; }
    }
}