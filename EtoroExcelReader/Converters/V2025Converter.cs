using System;
using System.Data;
using Database.Entities.InMemory;
using ExcelReader.Dictionaries.V2025;
using ExcelReader.ExtensionMethods;

namespace ExcelReader.Converters;

public sealed class V2025Converter : IRowToEntityConverter
{
    public ClosedPositionEntity ToClosedPositionEntity(DataRow row)
    {
        var openingConversionRate = row[ClosedPositionsColumnsV2025.OpeningConversionRate].ToDecimal();
        var closingConversionRate = row[ClosedPositionsColumnsV2025.ClosingConversionRate].ToDecimal();
        
        var openingRate = row[ClosedPositionsColumnsV2025.OpeningRate].ToDecimal();
        var closingRate = row[ClosedPositionsColumnsV2025.ClosingRate].ToDecimal();
        if (openingConversionRate != 0)
        {
            openingRate *= openingConversionRate;
        }
        if (closingConversionRate != 0)
        {
            closingRate *= closingConversionRate;
        }
        var closedPositionEntity = new ClosedPositionEntity
        {
            PositionId = row[ClosedPositionsColumnsV2025.PositionId].ToLong(),
            TransactionType = row[ClosedPositionsColumnsV2025.OperationType].ToTransactionType(),
            Operation = row[ClosedPositionsColumnsV2025.Operation].OperationToString(),
            Amount = row[ClosedPositionsColumnsV2025.Amount].ToDecimal(),
            OpeningRate = openingRate,
            ClosingRate = closingRate,
            Leverage = row[ClosedPositionsColumnsV2025.Leverage].ToInt(),
            OpeningDate = row[ClosedPositionsColumnsV2025.OpeningDate].ToDate(),
            ClosingDate = row[ClosedPositionsColumnsV2025.ClosingDate].ToDate(),
            IsReal = row[ClosedPositionsColumnsV2025.IsReal].ToString(),
            ISIN = row[ClosedPositionsColumnsV2025.ISIN].ToCountryName()
        };

        closedPositionEntity.Units = closedPositionEntity.Leverage == 1
            ?row[ClosedPositionsColumnsV2025.Units].ToDecimal()
            : Math.Round(
                (decimal)(closedPositionEntity.Amount * closedPositionEntity.Leverage /
                          closedPositionEntity.OpeningRate), 6, MidpointRounding.AwayFromZero);


        return closedPositionEntity;
    }

    public TransactionReportEntity ToTransactionReportEntity(DataRow row)
    {
        TransactionReportEntity transactionReportEntity = new TransactionReportEntity
        {
            Date = row[TransactionReportsColumnsV2025.Date].ToDate(),
            Type = row[TransactionReportsColumnsV2025.Type].ToString(),
            Details = row[TransactionReportsColumnsV2025.Details].ToString(),
            PositionId = row[TransactionReportsColumnsV2025.PositionId].ToLong(),
            Amount = row[TransactionReportsColumnsV2025.Amount].ToDecimal(),
            IsCryptocurrency = false
        };

        var transactionType = row[TransactionReportsColumnsV2025.IsReal].ToString();
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
            DateOfPayment = row[DividendColumnsV2025.DateOfPayment].ToDate(),
            InstrumentName = row[DividendColumnsV2025.InstrumentName].ToString(),
            NetDividendReceived = row[DividendColumnsV2025.NetDividendReceived].ToDecimal(),
            WithholdingTaxRate = row[DividendColumnsV2025.WithholdingTaxRate].ToDecimal(),
            WithholdingTaxAmount = row[DividendColumnsV2025.WithholdingTaxAmount].ToDecimal(),
            PositionId = row[DividendColumnsV2025.PositionId].ToLong(),
            PositionType = row[DividendColumnsV2025.PositionType].ToString(),
            ISIN = row[DividendColumnsV2025.ISIN].ToCountryName(),
        };

        return dividendEntity;
    }
}