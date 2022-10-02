using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities.InMemory
{
    [Table("Dividends")]
    public record DividendEntity
    {
        [Key] public int Id { get; set; }

        public long PositionId { get; set; }

        public DateTime DateOfPayment { get; set; }

        public string InstrumentName { get; set; }

        public decimal NetDividendReceived { get; set; }

        public decimal WithholdingTaxRate { get; set; }

        public decimal WithholdingTaxAmount { get; set; }

        public string PositionType { get; set; }

        public string ISIN { get; set; }
    }
}