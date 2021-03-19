using System;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace EtoroExcelReader.Dto
{
    public class TransactionReportExcelDto
    {

        public TransactionReportExcelDto(DataRow row)
        {
            Date = DateTime.Parse(row[0].ToString());
            
            AccountBalance = decimal.Parse(row[1].ToString());

            Type = row[2].ToString();

            Details = row[3].ToString();

            int.TryParse(row[4].ToString(), out int outPositonId);
            PositionId = outPositonId;

            Amount = decimal.Parse(row[5].ToString());

            RealizedEquityChange = decimal.Parse(row[6].ToString());

            RealizedEquity = decimal.Parse(row[7].ToString());

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
