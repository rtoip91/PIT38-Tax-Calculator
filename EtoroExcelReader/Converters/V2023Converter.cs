using System;
using System.Data;
using Database.Entities.InMemory;
using ExcelReader.Dictionaries.V2023;
using ExcelReader.ExtensionMethods;

namespace ExcelReader.Converters;

internal sealed class V2023Converter : IRowToEntityConverter
{
    public ClosedPositionEntity ToClosedPositionEntity(DataRow row)
    {
        ClosedPositionEntity closedPositionEntity = new ClosedPositionEntity
        {
            PositionId = row[ClosedPositionsColumnsV2023.PositionId].ToLong(),
            TransactionType = row[ClosedPositionsColumnsV2023.Operation].ToTransactionType(),
            Operation = row[ClosedPositionsColumnsV2023.Operation].OperationToString(),
            Amount = row[ClosedPositionsColumnsV2023.Amount].ToDecimal(),
            OpeningRate = row[ClosedPositionsColumnsV2023.OpeningRate].ToDecimal(),
            ClosingRate = row[ClosedPositionsColumnsV2023.ClosingRate].ToDecimal(),
            Leverage = row[ClosedPositionsColumnsV2023.Leverage].ToInt(),
            OpeningDate = row[ClosedPositionsColumnsV2023.OpeningDate].ToDate(),
            ClosingDate = row[ClosedPositionsColumnsV2023.ClosingDate].ToDate(),
            IsReal = row[ClosedPositionsColumnsV2023.IsReal].ToString(),
            ISIN = row[ClosedPositionsColumnsV2023.ISIN].ToCountryName()
        };

        closedPositionEntity.Units = closedPositionEntity.Leverage == 1
            ?row[ClosedPositionsColumnsV2023.Units].ToDecimal()
            : Math.Round(
                (decimal)(closedPositionEntity.Amount * closedPositionEntity.Leverage /
                          closedPositionEntity.OpeningRate), 6, MidpointRounding.AwayFromZero);


        return closedPositionEntity;
    }

    public TransactionReportEntity ToTransactionReportEntity(DataRow row)
    {
        TransactionReportEntity transactionReportEntity = new TransactionReportEntity
        {
            Date = row[TransactionReportsColumnsV2023.Date].ToDate(),
            Type = row[TransactionReportsColumnsV2023.Type].ToString(),
            Details = row[TransactionReportsColumnsV2023.Details].ToString(),
            PositionId = row[TransactionReportsColumnsV2023.PositionId].ToLong(),
            Amount = row[TransactionReportsColumnsV2023.Amount].ToDecimal(),
            IsCryptocurrency = false
        };

        var transactionType = row[TransactionReportsColumnsV2023.IsReal].ToString();
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
            DateOfPayment = row[DividendColumnsV2023.DateOfPayment].ToDate(),
            InstrumentName = row[DividendColumnsV2023.InstrumentName].ToString(),
            NetDividendReceived = row[DividendColumnsV2023.NetDividendReceived].ToDecimal(),
            WithholdingTaxRate = row[DividendColumnsV2023.WithholdingTaxRate].ToDecimal(),
            WithholdingTaxAmount = row[DividendColumnsV2023.WithholdingTaxAmount].ToDecimal(),
            PositionId = row[DividendColumnsV2023.PositionId].ToLong(),
            PositionType = row[DividendColumnsV2023.PositionType].ToString(),
            ISIN = row[DividendColumnsV2023.ISIN].ToCountryName(),
        };

        return dividendEntity;
    }
}