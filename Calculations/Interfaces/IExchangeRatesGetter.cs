using Database.Entities;

namespace Calculations.Interfaces
{
    public interface IExchangeRatesGetter
    {
        Task<ExchangeRateEntity> GetRateForPreviousDay(string currencyCode, DateTime date);
    }
}
