﻿using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<ClosedPositionEntity> ClosedPositions { get; set; }
        public DbSet<TransactionReportEntity> TransactionReports { get; set; }
        public DbSet<ExchangeRateEntity> ExchangeRates { get; set; }
        public DbSet<CfdEntity> CfdCalculations { get; set; }
        public DbSet<CryptoEntity> CryptoCalculations { get; set; }
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
            optionsBuilder.UseSqlite("DataSource=TaxCalculator.db;");
        }
    }
}
