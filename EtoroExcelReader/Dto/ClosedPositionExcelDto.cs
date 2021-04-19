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
            PositionId = int.Parse(row[0].ToString(), CultureInfo.InvariantCulture);

            Operation = row[1].ToString();

            CopiedInvestor = row[2].ToString();

            Amount = decimal.Parse(row[3].ToString(), CultureInfo.InvariantCulture);

            Units = decimal.Parse(row[4].ToString(), CultureInfo.InvariantCulture);

            OpeningRate = decimal.Parse(row[5].ToString(), CultureInfo.InvariantCulture);

            ClosingRate = decimal.Parse(row[6].ToString(), CultureInfo.InvariantCulture);

            Spread = decimal.Parse(row[7].ToString(), CultureInfo.InvariantCulture);

            Profit = decimal.Parse(row[8].ToString(), CultureInfo.InvariantCulture);

            OpeningDate = DateTime.Parse(row[9].ToString());

            ClosingDate = DateTime.Parse(row[10].ToString());

            TakeProfitRate = decimal.Parse(row[11].ToString(), CultureInfo.InvariantCulture);

            StopLossRate = decimal.Parse(row[12].ToString(), CultureInfo.InvariantCulture);

            FeesAndDividends = decimal.Parse(row[13].ToString(), CultureInfo.InvariantCulture);

            IsReal = row[14].ToString();

            int.TryParse(row[15].ToString(), out int outLeverage);
            Leverage = outLeverage;

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