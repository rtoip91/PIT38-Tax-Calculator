using ExcelReader.ExtensionMethods;
using System;
using System.Data;
using Database.Enums;
using ExcelReader.Dictionaries.V2021;

namespace EtoroExcelReader.Dto
{
    public record ClosedPositionExcelDto
    {
        public ClosedPositionExcelDto(DataRow row)
        {
            PositionId = row[ClosedPositionsColumnsV2021.PositionId].ToLong();

            TransactionType = row[ClosedPositionsColumnsV2021.Operation].ToTransactionType();

            Operation = row[ClosedPositionsColumnsV2021.Operation].OperationToString();

            CopiedInvestor = row[ClosedPositionsColumnsV2021.CopiedInvestor].ToString();

            Amount = row[ClosedPositionsColumnsV2021.Amount].ToDecimal();

            OpeningRate = row[ClosedPositionsColumnsV2021.OpeningRate].ToDecimal();

            ClosingRate = row[ClosedPositionsColumnsV2021.ClosingRate].ToDecimal();

            Leverage = row[ClosedPositionsColumnsV2021.Leverage].ToInt();

            Units = Math.Round((decimal)(Amount * Leverage / OpeningRate), 6, MidpointRounding.AwayFromZero);

            Spread = row[ClosedPositionsColumnsV2021.Spread].ToDecimal();

            Profit = row[ClosedPositionsColumnsV2021.Profit].ToDecimal();

            OpeningDate = row[ClosedPositionsColumnsV2021.OpeningDate].ToDate();

            ClosingDate = row[ClosedPositionsColumnsV2021.ClosingDate].ToDate();

            TakeProfitRate = row[ClosedPositionsColumnsV2021.TakeProfitRate].ToDecimal();

            StopLossRate = row[ClosedPositionsColumnsV2021.StopLossRate].ToDecimal();

            FeesAndDividends = row[ClosedPositionsColumnsV2021.FeesAndDividends].ToDecimal();

            IsReal = row[ClosedPositionsColumnsV2021.IsReal].ToString();

            Leverage = row[ClosedPositionsColumnsV2021.Leverage].ToInt();

            ISIN = row[ClosedPositionsColumnsV2021.ISIN].ToIso3166Symbol();
        }

        public long? PositionId { get; }

        public TransactionType TransactionType { get; }

        public string Operation { get; }

        public string CopiedInvestor { get; }

        public decimal? Amount { get; }

        public decimal? Units { get; }

        public decimal? OpeningRate { get; }

        public decimal? ClosingRate { get; }

        public decimal? Spread { get; }

        public decimal? Profit { get; }

        public DateTime OpeningDate { get; }

        public DateTime ClosingDate { get; }

        public decimal? TakeProfitRate { get; }

        public decimal? StopLossRate { get; }

        public decimal? FeesAndDividends { get; }

        public string IsReal { get; }

        public int Leverage { get; }

        public string ISIN { get; }
    }
}