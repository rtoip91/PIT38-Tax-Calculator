using System.IO;
using Database.Entities.Database;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    internal sealed class ApplicationDbContext : DbContext
    {
        public DbSet<ExchangeRateEntity> ExchangeRates { get; set; }
        public DbSet<FileEntity> FileEntities { get; set; }
        private static bool _isMigrated;
        private static readonly object locker = new();

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