using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities
{
    [Table("PurchasedCryptoCalculations")]
    public class PurchasedCryptoEntity
    {
        [Key] public int Id { get; set; }
        public long PositionId { get; set; }
        public string Name { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal Units { get; set; }
        public decimal ValuePerUnit { get; set; }
        public decimal TotalValue { get; set; }
        public string CurrencySymbol { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal TotalExchangedValue { get; set; }
    }
}
