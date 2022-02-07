using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities
{
    [Table("CfdCalculations")]
    public class CfdEntity
    {
        [Key] public int Id { get; set; }

        public string Name { get; set; }

        public long PositionId { get; set; }

        public DateTime PurchaseDate { get; set; }

        public DateTime SellDate { get; set; }

        public decimal Units { get; set; }

        public decimal OpeningRate { get; set; }

        public decimal ClosingRate { get; set; }

        public decimal GainValue { get; set; }

        public string CurrencySymbol { get; set; }

        public decimal ExchangeRate { get; set; }

        public decimal GainExchangedValue { get; set; }

        public decimal LossExchangedValue { get; set; }

        public override string ToString()
        {
            var exchangedGain = GainExchangedValue != 0 ? GainExchangedValue : LossExchangedValue;
            return $"{Name} | Ilość jednostek:{Units} |\nData zakupu:{PurchaseDate.ToShortDateString()} Cena:{OpeningRate}USD | Data sprzedaży:{SellDate.ToShortDateString()} Cena:{ClosingRate} USD | Wynik w dniu sprzedaży:{GainValue}USD | Kurs NBP z dnia poprzedniego: {ExchangeRate} | Wynik w PLN {exchangedGain} \n";
        }
    }
}