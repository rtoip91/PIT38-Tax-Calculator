using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities.InMemory
{
    [Table("StockCalculations")]
    public record StockEntity
    {
        [Key] public int Id { get; set; }

        public long PositionId { get; set; }

        public string Name { get; set; }

        public DateTime PurchaseDate { get; set; }

        public DateTime SellDate { get; set; }

        public string CurrencySymbol { get; set; }

        public decimal Units { get; set; }

        public decimal OpeningUnitValue { get; set; }

        public decimal ClosingUnitValue { get; set; }

        public decimal OpeningValue { get; set; }

        public decimal ClosingValue { get; set; }

        public decimal Profit { get; set; }

        public decimal ExchangedProfit { get; set; }

        public decimal OpeningExchangeRate { get; set; }

        public DateTime OpeningExchangeRateDate { get; set; }

        public decimal ClosingExchangeRate { get; set; }

        public DateTime ClosingExchangeRateDate { get; set; }

        public decimal ClosingExchangedValue { get; set; }

        public decimal OpeningExchangedValue { get; set; }

        public string Country { get; set; }

        public override string ToString()
        {
            return $"Operacja: {Name} | ID: {PositionId} | Kraj: {Country} |" +
                   $"\nIlość jednostek: {Units} |" +
                   $"\nData otwarcia: {PurchaseDate.ToShortDateString()} | Cena za jednostkę: {OpeningUnitValue} {CurrencySymbol} |" +
                   $"\nData zamknięcia: {SellDate.ToShortDateString()} | Cena za jednostkę: {ClosingUnitValue} {CurrencySymbol} |" +
                   $"\nWartość jednostek w dniu zakupu: {OpeningValue} {CurrencySymbol} | Kurs {CurrencySymbol} z dnia {OpeningExchangeRateDate.ToShortDateString()}: {OpeningExchangeRate} PLN | Po przeliczeniu: {OpeningExchangedValue} PLN" +
                   $"\nWartość jednostek w dniu sprzedaży: {ClosingValue} {CurrencySymbol} | Kurs {CurrencySymbol} z dnia {ClosingExchangeRateDate.ToShortDateString()}: {ClosingExchangeRate} PLN | Po przeliczeniu: {ClosingExchangedValue} PLN" +
                   $"\nWynik po przeliczeniu: {ExchangedProfit} PLN\n";
        }
    }
}