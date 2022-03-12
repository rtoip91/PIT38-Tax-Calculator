using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities
{
    [Table("DividendCalculations")]
    public  class DividendCalculationsEntity
    {
        [Key] public int Id { get; set; }

        public int PositionId { get; set; }
        public string InstrumentName { get; set; }

        public DateTime DateOfPayment { get; set; }

        public string Currency { get; set; }

        public decimal ExchangeRate { get; set; }

        public decimal NetDividendReceived { get; set; }

        public decimal NetDividendReceivedExchanged { get; set; }

        public decimal WithholdingTaxRate { get; set; }

        public decimal WithholdingTaxPaid { get; set; }

        public decimal WithholdingTaxRemain { get; set; }

        public string Country { get; set; }

        public override string ToString()
        {
            return $"Dywidenda za: {InstrumentName} | ID: {PositionId} | Kraj: {Country} |" +
                   $"\nData otrzymania: {DateOfPayment.ToShortDateString()} | Wartość : {NetDividendReceived} {Currency} |" +
                   $"\nKurs {Currency} z dnia poprzedniego: {ExchangeRate} PLN | Po przeliczeniu: {NetDividendReceivedExchanged} |" +
                   $"\nStawka podatku: {WithholdingTaxRate}% | Podatek zapłacony {WithholdingTaxPaid} PLN | Podatek do zapłaty {WithholdingTaxRemain} PLN |\n";
        }
    }
}
