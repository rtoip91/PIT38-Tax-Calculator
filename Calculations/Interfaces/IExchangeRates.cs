using Database.Entities;

namespace Calculations.Interfaces
{
    public interface IExchangeRates
    {
        Task<ExchangeRateEntity> GetRateForPreviousDay(string currencyCode, DateTime date);
    }
}
