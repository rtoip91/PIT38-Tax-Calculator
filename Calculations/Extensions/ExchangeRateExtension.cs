using Database.DataAccess;
using Database.DataAccess.Interfaces;
using Database.Entities;

namespace Calculations.Extensions
{
    internal static  class ExchangeRateExtension
    {
        private static readonly IExchangeRatesDataAccess _exchangeRatesDataAccess;

        static ExchangeRateExtension()
        {
            _exchangeRatesDataAccess = new ExchangeRatesDataAccess();
        }

        internal static async Task<ExchangeRateEntity> MakeCopyAndSaveToDb(this ExchangeRateEntity item)
        {
            ExchangeRateEntity newEntity = new ExchangeRateEntity 
            {
                Code = item.Code,
                Currency = item.Currency,
                Date = item.Date,
                Rate = item.Rate
            };

            await _exchangeRatesDataAccess.SaveRate(newEntity);
            return newEntity;
        }
    }
}
