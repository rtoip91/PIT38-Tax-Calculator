﻿using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    internal class ApplicationDbContext : DbContext
    {
        public DbSet<DividendEntity> Dividends { get; set; }
        public DbSet<DividendCalculationsEntity> DividendsCalculations { get; set; }
        public DbSet<ExchangeRateEntity> ExchangeRates { get; set; }
        public DbSet<CfdEntity> CfdCalculations { get; set; }
        public DbSet<PurchasedCryptoEntity> PurchasedCryptoCalculations { get; set; }
        public DbSet<SoldCryptoEntity> SoldCryptoCalculations { get; set; }
        public DbSet<StockEntity> StockCalculations { get; set; }

        public ApplicationDbContext()
        {
            Database.Migrate();
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("DataSource=EtoroTaxCalculator.db;");
        }
    }
}