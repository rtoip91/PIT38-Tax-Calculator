using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities
{
    [Table("CfdCalculations")]
    public class CfdEntity
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime PurchaseDate { get; set; }

        public DateTime SellDate { get; set; }

        public decimal Units { get; set; }

        public decimal OpeningRate { get; set; }

        public decimal ClosingRate { get; set; }

        public decimal GainValue { get; set; }

        public string CurrencySymbol { get; set; }

        public decimal ExchangeRate { get; set; }

        public decimal GainExchangedValue { get; set; }     

    }
}
