using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    internal sealed class ApplicationDbContext : DbContext
    {
        public DbSet<ExchangeRateEntity> ExchangeRates { get; set; }
        private static bool _isMigrated = false;
        private static object locker = new();

        public ApplicationDbContext()
        {
            MigrateDatabase();
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            MigrateDatabase();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("DataSource=TaxCalculator.db;");
        }

        private void MigrateDatabase()
        {
            lock (locker)
            {
                if (!_isMigrated)
                {
                    Database.Migrate();
                    _isMigrated = true;
                }
            }
        }
    }
}