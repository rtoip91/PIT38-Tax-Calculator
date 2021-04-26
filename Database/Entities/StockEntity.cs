using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities
{
    [Table("StockCalculations")]
    public class StockEntity
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime PurchaseDate { get; set; }

        public DateTime SellDate { get; set; }

        public decimal Units { get; set; }

        public decimal OpeningRate { get; set; }

        public decimal ClosingRate { get; set; }

        public string CurrencySymbol { get; set; }

        public decimal OpeningExchangeRate { get; set; }

        public decimal ClosingExchangeRate { get; set; }

        public decimal GainExchangedValue { get; set; }

        public decimal LossExchangedValue { get; set; }
    }
}
