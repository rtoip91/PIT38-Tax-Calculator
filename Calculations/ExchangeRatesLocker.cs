using Calculations.Interfaces;

namespace Calculations
{
    internal class ExchangeRatesLocker : IExchangeRatesLocker
    {
        private readonly Dictionary<DateTime, SemaphoreSlim> lockers = new Dictionary<DateTime, SemaphoreSlim>();

        public SemaphoreSlim GetLocker(DateTime exchangeRateDate)
        {
            lock (lockers)
            {
                if (!lockers.ContainsKey(exchangeRateDate))
                {
                    lockers.Add(exchangeRateDate, new SemaphoreSlim(1, 1));
                }

                return lockers[exchangeRateDate];
            }
        }
    }
}
