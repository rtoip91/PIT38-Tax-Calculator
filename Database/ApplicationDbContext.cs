using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    internal class ApplicationDbContext : DbContext
    {
        public DbSet<ClosedPositionEntity> ClosedPositions { get; set; }
        public DbSet<TransactionReportEntity> TransactionReports { get; set; }
        public DbSet<ExchangeRateEntity> ExchangeRates { get; set; }
        public DbSet<CfdEntity> CfdCalculations { get; set; }
        public DbSet<CryptoEntity> CryptoCalculations { get; set; }
        public DbSet<StockEntity> StockCalculations { get; set; }


        public ApplicationDbContext()
        {
            Database.EnsureCreated();
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("DataSource=TaxCalculator.db;");
        }
    }
}