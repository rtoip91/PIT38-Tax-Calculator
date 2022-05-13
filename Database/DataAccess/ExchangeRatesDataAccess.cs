using System;
using System.Linq;
using System.Threading.Tasks;
using Database.DataAccess.Interfaces;
using Database.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace Database.DataAccess
{
    public class ExchangeRatesDataAccess : IExchangeRatesDataAccess
    {
        private readonly IMemoryCache _memoryCache;

        public ExchangeRatesDataAccess(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task<ExchangeRateEntity> GetRate(string currencyCode, DateTime date)
        {
            await using var context = new ApplicationDbContext();
            var exchangeRate = _memoryCache.Get<ExchangeRateEntity>($"{currencyCode}-{date.Date.ToShortDateString()}");
            if (exchangeRate == null)
            {
                exchangeRate =
                    context.ExchangeRates.FirstOrDefault(rate =>
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
            ExchangeRateEntity newEntity = new ExchangeRateEntity
            {
                Code = item.Code,
                Currency = item.Currency,
                Date = item.Date,
                Rate = item.Rate
            };

            await SaveRate(newEntity);
            return newEntity;
        }

        public async Task<int> SaveRate(ExchangeRateEntity exchangeRate)
        {
            await using var context = new ApplicationDbContext();
            await context.AddAsync(exchangeRate);
            _memoryCache.Set($"{exchangeRate.Code}-{exchangeRate.Date.Date.ToShortDateString()}", exchangeRate,
                TimeSpan.FromMinutes(2));
            return await context.SaveChangesAsync();
        }
    }
}