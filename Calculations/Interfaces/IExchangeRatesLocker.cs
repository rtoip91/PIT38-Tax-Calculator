namespace Calculations.Interfaces
{
    /// <summary>
    /// Interface for managing locks on exchange rates.
    /// </summary>
    public interface IExchangeRatesLocker
    {
        /// <summary>
        /// Retrieves a semaphore that can be used to lock access to the exchange rate for a specific date.
        /// </summary>
        /// <param name="exchangeRateDate">The date of the exchange rate to lock access to.</param>
        /// <returns>A SemaphoreSlim that can be used to lock and unlock access to the exchange rate for the specified date.</returns>
        SemaphoreSlim GetLocker(DateTime exchangeRateDate);

        /// <summary>
        /// Clears all existing locks.
        /// </summary>
        void ClearLockers();
    }
}