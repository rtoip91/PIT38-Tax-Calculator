using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<ClosedPositionEntity> ClosedPositions { get; set; }
        public DbSet<TransactionReportEntity> TransactionReports { get; set; }
        public DbSet<ExchangeRateEntity> ExchangeRates { get; set; }

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
            optionsBuilder.UseSqlite("DataSource=EtoroTaxCalculatorDB.db;");
        }
    }
}
