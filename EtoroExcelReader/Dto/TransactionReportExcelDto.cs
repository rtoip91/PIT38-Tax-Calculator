using ExcelReader.ExtensionMethods;
using System;
using System.Data;
using System.Globalization;

namespace EtoroExcelReader.Dto
{
    public class TransactionReportExcelDto
    {

        public TransactionReportExcelDto(DataRow row)
        {
           
            Date = DateTime.Parse(row[0].ToString());
            
            AccountBalance = row[6].ToDecimal();

            Type = row[1].ToString();

            Details = row[2].ToString();
           
            PositionId = row[7].ToInt();

           // Amount = decimal.Parse(row[6].ToString(), provider);                  
            
            RealizedEquityChange = row[4].ToDecimal();

            RealizedEquity = row[5].ToDecimal();

            NWA = row[8].ToInt();
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
