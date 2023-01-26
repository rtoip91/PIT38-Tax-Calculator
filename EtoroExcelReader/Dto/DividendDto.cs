using System;
using System.Data;
using ExcelReader.Dictionaries.V2021;
using ExcelReader.ExtensionMethods;

namespace ExcelReader.Dto
{
    public record DividendDto
    {
        public DividendDto(DataRow row)
        {
            DateOfPayment = row[DividendsColumnsV2021.DateOfPayment].ToDate();
            InstrumentName = row[DividendsColumnsV2021.InstrumentName].ToString();
            NetDividendReceived = row[DividendsColumnsV2021.NetDividendReceived].ToDecimal();
            WithholdingTaxRate = row[DividendsColumnsV2021.WithholdingTaxRate].ToDecimal();
            WithholdingTaxAmount = row[DividendsColumnsV2021.WithholdingTaxAmount].ToDecimal();
            PositionId = row[DividendsColumnsV2021.PositionId].ToLong();
            PositionType = row[DividendsColumnsV2021.PositionType].ToString();
            ISIN = row[DividendsColumnsV2021.ISIN].ToIso3166Symbol();
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