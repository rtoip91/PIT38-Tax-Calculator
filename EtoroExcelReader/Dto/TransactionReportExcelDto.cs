using System;
using System.Data;
using System.Globalization;

namespace EtoroExcelReader.Dto
{
    public class TransactionReportExcelDto
    {

        public TransactionReportExcelDto(DataRow row)
        {
            CultureInfo provider = new CultureInfo("pl-PL");

            Date = DateTime.Parse(row[0].ToString());
            
            AccountBalance = decimal.Parse(row[1].ToString(), provider);

            Type = row[2].ToString();

            Details = row[3].ToString();

            int.TryParse(row[4].ToString(), out int outPositonId);
            PositionId = outPositonId;

            Amount = decimal.Parse(row[5].ToString(), provider);                  
            
            RealizedEquityChange = decimal.Parse(row[6].ToString(), provider);

            RealizedEquity = decimal.Parse(row[7].ToString(), provider);

            NWA = int.Parse(row[8].ToString());
        }
        
        public DateTime Date { get;}

        public decimal AccountBalance { get; }

        public string Type { get; }

        public string Details { get; }

        public int PositionId { get; }

        public decimal Amount { get; }
        
        public decimal RealizedEquityChange { get; }
        
        public decimal RealizedEquity { get; }
        
        public int NWA { get; }
    }
}
