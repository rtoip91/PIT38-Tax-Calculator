using ExcelReader.Dictionatries;
using ExcelReader.ExtensionMethods;
using System;
using System.Data;

namespace EtoroExcelReader.Dto
{
    public class TransactionReportExcelDto
    {
        public TransactionReportExcelDto(DataRow row)
        {
            Date = DateTime.Parse(row[TransactionReportsColumns.Date].ToString());

            AccountBalance = row[TransactionReportsColumns.AccountBalance].ToDecimal();

            Type = row[TransactionReportsColumns.Type].ToString();

            Details = row[TransactionReportsColumns.Details].ToString();

            PositionId = row[TransactionReportsColumns.PositionId].ToLong();

            Amount = row[TransactionReportsColumns.Amount].ToDecimal();

            RealizedEquityChange = row[TransactionReportsColumns.RealizedEquityChange].ToDecimal();

            RealizedEquity = row[TransactionReportsColumns.RealizedEquity].ToDecimal();

            NWA = row[TransactionReportsColumns.NWA].ToInt();
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
    }
}