using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EtoroExcelReader.Entities
{
    [Table("TransactionReports")]
    class TransactionReportEntity
    {
        [Key]
        public int Id { get; set; }
        
        public DateTime Date { get; set; }

        public decimal AccountBalance { get; set; }

        public string Type { get; set; }

        public string Details { get; set; }

        [Required]
        public int PositionId { get; set; }
        
        [ForeignKey("PositionId")]
        public ClosedPositionEntity ClosedPosition { get; set; }

        public decimal Amount { get; set; }

        public decimal RealizedEquityChange { get; set; }

        public decimal RealizedEquity { get; set; }

        public int NWA { get; set; }
    }
}
