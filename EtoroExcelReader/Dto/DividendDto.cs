using System;
using System.Data;
using ExcelReader.Dictionatries;
using ExcelReader.ExtensionMethods;

namespace ExcelReader.Dto
{
    public record DividendDto
    {
        public DividendDto(DataRow row)
        {
            DateOfPayment = row[DividendsColumns.DateOfPayment].ToDate();
            InstrumentName = row[DividendsColumns.InstrumentName].ToString();
            NetDividendReceived = row[DividendsColumns.NetDividendReceived].ToDecimal();
            WithholdingTaxRate = row[DividendsColumns.WithholdingTaxRate].ToDecimal();
            WithholdingTaxAmount = row[DividendsColumns.WithholdingTaxAmount].ToDecimal();
            PositionId = row[DividendsColumns.PositionId].ToLong();
            PositionType = row[DividendsColumns.PositionType].ToString();
            ISIN = row[DividendsColumns.ISIN].ToIso3166Symbol();
        }

        public DateTime DateOfPayment { get; }
        public string InstrumentName { get; }
        public decimal NetDividendReceived { get; }
        public decimal WithholdingTaxRate { get; }
        public decimal WithholdingTaxAmount { get; }
        public long PositionId { get; }
        public string PositionType { get; }
        public string ISIN { get; }
    }
}