using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities.Database
{
    [Table("ExchangeRates")]
    public class ExchangeRateEntity
    {
        [Key] public int Id { get; set; }

        public string Currency { get; set; }

        public string Code { get; set; }

        public DateTime Date { get; set; }

        public decimal Rate { get; set; }
    }
}