using System.IO;
using Database.Entities.Database;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    internal sealed class ApplicationDbContext : DbContext
    {
        public DbSet<ExchangeRateEntity> ExchangeRates { get; set; }
        public DbSet<FileEntity> FileEntities { get; set; }
        public DbSet<CryptocurrencyEntity> CryptocurrencyEntities { get; set; }
        private static bool _isMigrated;
        private readonly object _locker = new();

        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("DataSource=TaxCalculator.db;");
        }

        internal void MigrateDatabase()
        {
            lock (_locker)
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