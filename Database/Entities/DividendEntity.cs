using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Entities
{
    [Table("Dividends")]
    public class DividendEntity
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