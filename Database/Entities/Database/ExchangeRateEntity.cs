using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database.Entities.Database
{
    [Table("ExchangeRates")]
    [Index(nameof(Code),nameof(Date))]
    public record ExchangeRateEntity
    {
        [Key] public int Id { get; set; }

        public string Currency { get; set; }

        public string Code { get; set; }

        public DateTime Date { get; set; }

        public decimal Rate { get; set; }
    }
}