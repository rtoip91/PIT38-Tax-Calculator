using ExcelReader.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Calculations.Dictionaries;
using ExcelReader.Dictionaries.V2021;

namespace EtoroExcelReader.Dto
{
    public record TransactionReportExcelDto
    {
        public TransactionReportExcelDto(DataRow row)
        {
            Date = DateTime.Parse(row[TransactionReportsColumnsV2021.Date].ToString());

            AccountBalance = row[TransactionReportsColumnsV2021.AccountBalance].ToDecimal();

            Type = row[TransactionReportsColumnsV2021.Type].ToString();

            Details = row[TransactionReportsColumnsV2021.Details].ToString();

            PositionId = row[TransactionReportsColumnsV2021.PositionId].ToLong();

            Amount = row[TransactionReportsColumnsV2021.Amount].ToDecimal();

            RealizedEquityChange = row[TransactionReportsColumnsV2021.RealizedEquityChange].ToDecimal();

            RealizedEquity = row[TransactionReportsColumnsV2021.RealizedEquity].ToDecimal();

            NWA = row[TransactionReportsColumnsV2021.NWA].ToInt();

            IsCryptoCurrency = false;

            if (Details != null && Type != null)
            {
                if(Type.ToLower().Contains("Otwarta pozycja".ToLower()))
                { 
                    string name = Details.Split('/').FirstOrDefault();
                    var result =  Dictionaries.CryptoCurrenciesDictionary.TryGetValue(name, out _);
                    IsCryptoCurrency = result;
                }
            }
        }

        public DateTime Date { get; }

        public decimal AccountBalance { get; }

        public string Type { get; }

        public string Details { get; }

        public long PositionId { get; }

        public decimal Amount { get; }

        public decimal RealizedEquityChange { get; }

        public decimal RealizedEquity { get; }

        public int NWA { get; }
        
        public bool IsCryptoCurrency { get; }
    }
}