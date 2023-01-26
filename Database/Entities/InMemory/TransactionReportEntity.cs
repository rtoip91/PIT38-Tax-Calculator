using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities.InMemory
{
    [Table("TransactionReports")]
    public record TransactionReportEntity
    {
        [Key] public int Id { get; set; }

        public DateTime Date { get; set; }

        public decimal AccountBalance { get; set; }

        public string Type { get; set; }

        public string Details { get; set; }

        public long? PositionId { get; set; }

        [ForeignKey("PositionId")] public ClosedPositionEntity ClosedPosition { get; set; }

        public decimal Amount { get; set; }

        public decimal RealizedEquityChange { get; set; }

        public decimal RealizedEquity { get; set; }

        public int NWA { get; set; }
        
        public bool IsCryptocurrency { get; set; }
    }
}