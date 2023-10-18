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
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseIdentityColumns();
            modelBuilder.Entity<FileEntity>().Property(b => b.Id).UseIdentityAlwaysColumn();
            modelBuilder.Entity<ExchangeRateEntity>().Property(b => b.Id).UseIdentityAlwaysColumn();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(@"Host=172.17.0.2;Username=postgres;Password=docker;Database=postgres");
        }

        internal void MigrateDatabase()
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