using Database;
using Database.Entities;

namespace Calculations.Extensions
{
    internal static  class ExchangeRateExtension
    {
        internal static async Task<ExchangeRateEntity> MakeCopyAndSaveToDb(this ExchangeRateEntity item)
        {
            using var context = new ApplicationDbContext();
            ExchangeRateEntity newEntity = new ExchangeRateEntity 
            {
                Code = item.Code,
                Currency = item.Currency,
                Date = item.Date,
                Rate = item.Rate
            };

            await context.AddAsync(newEntity);
            await context.SaveChangesAsync();

            return newEntity;
        }
    }
}
