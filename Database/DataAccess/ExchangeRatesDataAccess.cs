using System;
using System.Linq;
using System.Threading.Tasks;
using Database.DataAccess.Interfaces;
using Database.Entities;

namespace Database.DataAccess
{
    public class ExchangeRatesDataAccess : IExchangeRatesDataAccess
    {
        public async Task<ExchangeRateEntity> GetRate(string currencyCode, DateTime date)
        {
            await using var context = new ApplicationDbContext();
            var exchangeRate = context.ExchangeRates.FirstOrDefault(rate => rate.Code == currencyCode && rate.Date.Date == date.Date);
            return exchangeRate;
        }

        public async Task<int> SaveRate(ExchangeRateEntity exchangeRate)
        {
            await using var context = new ApplicationDbContext();
            await context.AddAsync(exchangeRate);
            return await context.SaveChangesAsync();
        }
    }
}
