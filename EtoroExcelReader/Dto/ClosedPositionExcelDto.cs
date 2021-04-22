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
            CultureInfo provider;
            if (row[3].ToString().Contains('.'))
            {
                 provider = CultureInfo.InvariantCulture;
            }
            else
            {
                provider = new CultureInfo("pl-PL");
            }
           

            PositionId = int.Parse(row[0].ToString(), provider);

            Operation = row[1].ToString();

            CopiedInvestor = row[2].ToString();

            Amount = decimal.Parse(row[3].ToString(), provider);

            Units = decimal.Parse(row[4].ToString(), provider);

            OpeningRate = decimal.Parse(row[5].ToString(), provider);

            ClosingRate = decimal.Parse(row[6].ToString(), provider);

            Spread = decimal.Parse(row[7].ToString(), provider);

            Profit = decimal.Parse(row[8].ToString(), provider);

            OpeningDate = DateTime.Parse(row[9].ToString());

            ClosingDate = DateTime.Parse(row[10].ToString());

            TakeProfitRate = decimal.Parse(row[11].ToString(), provider);

            StopLossRate = decimal.Parse(row[12].ToString(), provider);

            FeesAndDividends = decimal.Parse(row[13].ToString(), provider);

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