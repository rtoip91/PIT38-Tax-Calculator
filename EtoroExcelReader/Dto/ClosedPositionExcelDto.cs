using ExcelReader.ExtensionMethods;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;

namespace EtoroExcelReader.Dto
{
    public class ClosedPositionExcelDto
    {
        public ClosedPositionExcelDto(DataRow row)
        {            
            PositionId = row[0].ToInt();

            Operation = row[1].ToString();

            CopiedInvestor = row[14].ToString();

            Amount =row[2].ToDecimal();         

            Units = row[3].ToDecimal();

            OpeningRate = row[9].ToDecimal();

            ClosingRate = row[10].ToDecimal();

            Spread = row[7].ToDecimal();
             
            Profit = row[8].ToDecimal();

            OpeningDate = row[4].ToDate();

            ClosingDate = row[5].ToDate();

            TakeProfitRate = row[11].ToDecimal();

            StopLossRate = row[12].ToDecimal();

            FeesAndDividends = row[13].ToDecimal();

            IsReal = row[15].ToString();

            Leverage = row[6].ToInt();

            Comments = row[16].ToString();
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