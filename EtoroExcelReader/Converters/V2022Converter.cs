using System;
using System.Data;
using Database.Entities.InMemory;
using ExcelReader.Dictionaries.V2022;
using ExcelReader.ExtensionMethods;

namespace ExcelReader.Converters;

internal sealed class V2022Converter : IRowToEntityConverter
{
    public ClosedPositionEntity ToClosedPositionEntity(DataRow row)
    {
        ClosedPositionEntity closedPositionEntity = new ClosedPositionEntity
        {
            PositionId = row[ClosedPositionsColumnsV2022.PositionId].ToLong(),
            TransactionType = row[ClosedPositionsColumnsV2022.Operation].ToTransactionType(),
            Operation = row[ClosedPositionsColumnsV2022.Operation].OperationToString(),
            Amount = row[ClosedPositionsColumnsV2022.Amount].ToDecimal(),
            OpeningRate = row[ClosedPositionsColumnsV2022.OpeningRate].ToDecimal(),
            ClosingRate = row[ClosedPositionsColumnsV2022.ClosingRate].ToDecimal(),
            Leverage = row[ClosedPositionsColumnsV2022.Leverage].ToInt(),
            OpeningDate = row[ClosedPositionsColumnsV2022.OpeningDate].ToDate(),
            ClosingDate = row[ClosedPositionsColumnsV2022.ClosingDate].ToDate(),
            IsReal = row[ClosedPositionsColumnsV2022.IsReal].ToString(),
            ISIN = row[ClosedPositionsColumnsV2022.ISIN].ToCountryName()
        };

        closedPositionEntity.Units = Math.Round((decimal)(closedPositionEntity.Amount * closedPositionEntity.Leverage / closedPositionEntity.OpeningRate), 6, MidpointRounding.AwayFromZero);

        return closedPositionEntity;

    }

    public TransactionReportEntity ToTransactionReportEntity(DataRow row)
    {
        TransactionReportEntity transactionReportEntity = new TransactionReportEntity
        {
            Date = row[TransactionReportsColumnsV2022.Date].ToDate(),
            Type = row[TransactionReportsColumnsV2022.Type].ToString(),
            Details = row[TransactionReportsColumnsV2022.Details].ToString(),
            PositionId = row[TransactionReportsColumnsV2022.PositionId].ToLong(),
            Amount = row[TransactionReportsColumnsV2022.Amount].ToDecimal(),
            IsCryptocurrency = false
        };
        
        var transactionType = row[TransactionReportsColumnsV2022.IsReal].ToString();
        if (transactionType != null && transactionType.Contains("Kryptoaktywa"))
        {
            transactionReportEntity.IsCryptocurrency = true;
        }
        
        return transactionReportEntity;
    }

    public DividendEntity ToDividendEntity(DataRow row)
    {
        DividendEntity dividendEntity = new DividendEntity
        {
            DateOfPayment = row[DividendsColumnsV2022.DateOfPayment].ToDate(),
            InstrumentName = row[DividendsColumnsV2022.InstrumentName].ToString(),
            NetDividendReceived = row[DividendsColumnsV2022.NetDividendReceived].ToDecimal(),
            WithholdingTaxRate = row[DividendsColumnsV2022.WithholdingTaxRate].ToDecimal(),
            WithholdingTaxAmount = row[DividendsColumnsV2022.WithholdingTaxAmount].ToDecimal(),
            PositionId = row[DividendsColumnsV2022.PositionId].ToLong(),
            PositionType = row[DividendsColumnsV2022.PositionType].ToString(),
            ISIN = row[DividendsColumnsV2022.ISIN].ToCountryName(),
        };

        return dividendEntity;
    }
}