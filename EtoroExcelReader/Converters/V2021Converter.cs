using System;
using System.Data;
using System.Linq;
using Database.Entities.InMemory;
using ExcelReader.Dictionaries.V2021;
using ExcelReader.ExtensionMethods;
using ExcelReader.Interfaces;

namespace ExcelReader.Converters;

internal class V2021Converter : IRowToEntityConverter
{
    public ClosedPositionEntity ToClosedPositionEntity(DataRow row)
    {
        ClosedPositionEntity closedPositionEntity = new ClosedPositionEntity
        {
            PositionId = row[ClosedPositionsColumnsV2021.PositionId].ToLong(),
            TransactionType = row[ClosedPositionsColumnsV2021.Operation].ToTransactionType(),
            Operation = row[ClosedPositionsColumnsV2021.Operation].OperationToString(),
            Amount = row[ClosedPositionsColumnsV2021.Amount].ToDecimal(),
            OpeningRate = row[ClosedPositionsColumnsV2021.OpeningRate].ToDecimal(),
            ClosingRate = row[ClosedPositionsColumnsV2021.ClosingRate].ToDecimal(),
            Leverage = row[ClosedPositionsColumnsV2021.Leverage].ToInt(),
            OpeningDate = row[ClosedPositionsColumnsV2021.OpeningDate].ToDate(),
            ClosingDate = row[ClosedPositionsColumnsV2021.ClosingDate].ToDate(),
            IsReal = row[ClosedPositionsColumnsV2021.IsReal].ToString(),
            ISIN = row[ClosedPositionsColumnsV2021.ISIN].ToIso3166Symbol()
        };


        closedPositionEntity.Units = Math.Round((decimal)(closedPositionEntity.Amount * closedPositionEntity.Leverage / closedPositionEntity.OpeningRate), 6, MidpointRounding.AwayFromZero);

        return closedPositionEntity;

    }

    public TransactionReportEntity ToTransactionReportEntity(DataRow row)
    {
        TransactionReportEntity transactionReportEntity = new TransactionReportEntity
        {
            Date = row[TransactionReportsColumnsV2021.Date].ToDate(),
            Type = row[TransactionReportsColumnsV2021.Type].ToString(),
            Details = row[TransactionReportsColumnsV2021.Details].ToString(),
            PositionId = row[TransactionReportsColumnsV2021.PositionId].ToLong(),
            Amount = row[TransactionReportsColumnsV2021.Amount].ToDecimal(),
            IsCryptocurrency = false
        };

        if (transactionReportEntity.Details != null)
        {
            string name = transactionReportEntity.Details.Split('/').FirstOrDefault();
            if (name != null)
            {
                var result =  Calculations.Dictionaries.Dictionaries.CryptoCurrenciesDictionary.TryGetValue(name, out _);
                transactionReportEntity.IsCryptocurrency = result;
            }
        }

        return transactionReportEntity;
    }

    public DividendEntity ToDividendEntity(DataRow row)
    {
        DividendEntity dividendEntity = new DividendEntity
        {
            DateOfPayment = row[DividendsColumnsV2021.DateOfPayment].ToDate(),
            InstrumentName = row[DividendsColumnsV2021.InstrumentName].ToString(),
            NetDividendReceived = row[DividendsColumnsV2021.NetDividendReceived].ToDecimal(),
            WithholdingTaxRate = row[DividendsColumnsV2021.WithholdingTaxRate].ToDecimal(),
            WithholdingTaxAmount = row[DividendsColumnsV2021.WithholdingTaxAmount].ToDecimal(),
            PositionId = row[DividendsColumnsV2021.PositionId].ToLong(),
            PositionType = row[DividendsColumnsV2021.PositionType].ToString(),
            ISIN = row[DividendsColumnsV2021.ISIN].ToIso3166Symbol(),
        };

        return dividendEntity;
    }
}