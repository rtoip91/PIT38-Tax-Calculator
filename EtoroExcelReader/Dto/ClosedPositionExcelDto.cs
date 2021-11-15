using ExcelReader.Dictionatries;
using ExcelReader.ExtensionMethods;
using System;
using System.Data;

namespace EtoroExcelReader.Dto
{
    public class ClosedPositionExcelDto
    {
        public ClosedPositionExcelDto(DataRow row)
        {            
            PositionId = row[ClosedPositionsColumns.PositionId].ToInt();

            Operation = row[ClosedPositionsColumns.Operation].ToString();

            CopiedInvestor = row[ClosedPositionsColumns.CopiedInvestor].ToString();

            Amount =row[ClosedPositionsColumns.Amount].ToDecimal();             

            OpeningRate = row[ClosedPositionsColumns.OpeningRate].ToDecimal();

            ClosingRate = row[ClosedPositionsColumns.ClosingRate].ToDecimal();

            Leverage = row[ClosedPositionsColumns.Leverage].ToInt();
            
            Units = Math.Round((decimal)(Amount * Leverage / OpeningRate), 6);

            Spread = row[ClosedPositionsColumns.Spread].ToDecimal();
             
            Profit = row[ClosedPositionsColumns.Profit].ToDecimal();

            OpeningDate = row[ClosedPositionsColumns.OpeningDate].ToDate();

            ClosingDate = row[ClosedPositionsColumns.ClosingDate].ToDate();

            TakeProfitRate = row[ClosedPositionsColumns.TakeProfitRate].ToDecimal();

            StopLossRate = row[ClosedPositionsColumns.StopLossRate].ToDecimal();

            FeesAndDividends = row[ClosedPositionsColumns.FeesAndDividends].ToDecimal();

            IsReal = row[ClosedPositionsColumns.IsReal].ToString();

            Leverage = row[ClosedPositionsColumns.Leverage].ToInt();

            Comments = row[ClosedPositionsColumns.Comments].ToString();
        }

        public int? PositionId { get; }

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

        public string Comments { get; }
    }
}