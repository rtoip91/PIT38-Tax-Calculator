using Database.Entities.Database;

namespace Calculations.Interfaces
{
    /// <summary>
    /// Interface for retrieving exchange rates.
    /// </summary>
    public interface IExchangeRates
    {
        /// <summary>
        /// Retrieves the exchange rate for a specific currency for the day before the provided date.
        /// </summary>
        /// <param name="currencyCode">The code of the currency to retrieve the exchange rate for.</param>
        /// <param name="date">The date to retrieve the exchange rate for the day before.</param>
        /// <returns>A Task that represents the asynchronous operation. The Task's result is an ExchangeRateEntity that contains the exchange rate information.</returns>
        Task<ExchangeRateEntity> GetRateForPreviousDay(string currencyCode, DateTime date);
    }
}