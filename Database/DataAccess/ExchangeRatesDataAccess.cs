using System;
using System.Linq;
using System.Threading.Tasks;
using Database.DataAccess.Interfaces;
using Database.Entities.Database;
using Microsoft.Extensions.Caching.Memory;

namespace Database.DataAccess
{
    public sealed class ExchangeRatesDataAccess : IExchangeRatesDataAccess
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ApplicationDbContext _context;

        public ExchangeRatesDataAccess(IMemoryCache memoryCache, ApplicationDbContext context)
        {
            _context = context;
            _memoryCache = memoryCache;
        }
        

        public async Task<ExchangeRateEntity> GetRate(string currencyCode, DateTime date)
        {
           
            var exchangeRate = _memoryCache.Get<ExchangeRateEntity>($"{currencyCode}-{date.Date.ToShortDateString()}");
            if (exchangeRate == null)
            {
                exchangeRate =
                    _context.ExchangeRates.FirstOrDefault(rate =>
                        rate.Code == currencyCode && rate.Date.Date == date.Date);
                if (exchangeRate != null)
                {
                    _memoryCache.Set($"{currencyCode}-{date.Date.ToShortDateString()}", exchangeRate,
                        TimeSpan.FromMinutes(2));
                }
            }

            return exchangeRate;
        }

        public async Task<ExchangeRateEntity> MakeCopyAndSaveToDb(ExchangeRateEntity item)
        {
            //non destructive record copy
            ExchangeRateEntity newEntity = item with { Id = 0};
            await SaveRate(newEntity);
            return newEntity;
        }

        public async Task<int> SaveRate(ExchangeRateEntity exchangeRate)
        {
            await _context.AddAsync(exchangeRate);
            _memoryCache.Set($"{exchangeRate.Code}-{exchangeRate.Date.Date.ToShortDateString()}", exchangeRate,
                TimeSpan.FromMinutes(2));
            return await _context.SaveChangesAsync();
        }
    }
}