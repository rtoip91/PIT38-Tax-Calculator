using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    internal class ApplicationDbContext : DbContext
    {
        public DbSet<ExchangeRateEntity> ExchangeRates { get; set; }

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