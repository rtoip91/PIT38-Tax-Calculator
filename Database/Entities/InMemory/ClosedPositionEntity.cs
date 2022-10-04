using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Database.Enums;

namespace Database.Entities.InMemory
{
    [Table("ClosedPositions")]
    public record ClosedPositionEntity
    {
        [Key] public long? PositionId { get; set; }

        public string Operation { get; set; }

        public TransactionType TransactionType { get; set; }

        public string CopiedInvestor { get; set; }

        public decimal? Amount { get; set; }

        public decimal? Units { get; set; }

        public decimal? OpeningRate { get; set; }

        public decimal? ClosingRate { get; set; }

        public decimal? Spread { get; set; }

        public decimal? Profit { get; set; }

        public DateTime OpeningDate { get; set; }

        public DateTime ClosingDate { get; set; }

        public decimal? TakeProfitRate { get; set; }

        public decimal? StopLossRate { get; set; }

        public decimal? FeesAndDividends { get; set; }

        public string IsReal { get; set; }

        public int Leverage { get; set; }

        public string ISIN { get; set; }

        public IList<TransactionReportEntity> TransactionReports { get; set; }
    }
}