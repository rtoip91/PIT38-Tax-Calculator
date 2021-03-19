using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EtoroExcelReader.Entities
{
    [Table("ExchangeRates")]
    internal class ExchangeRateEntity
    {
        [Key]
        public int Id { get; set; }
        
        public string Currency { get; set; }

        public string Code { get; set; }

        public DateTime Date { get; set; }

        public decimal Rate { get; set; }
    }
}
