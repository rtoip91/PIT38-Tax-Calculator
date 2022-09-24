using Calculations.Interfaces;
using System.Collections.Generic;

namespace Calculations
{
    internal class ExchangeRatesLocker : IExchangeRatesLocker
    {
        private readonly Dictionary<DateTime, SemaphoreSlim> _lockers = new Dictionary<DateTime, SemaphoreSlim>();

        public SemaphoreSlim GetLocker(DateTime exchangeRateDate)
        {
            lock (_lockers)
            {
                if (!_lockers.ContainsKey(exchangeRateDate))
                {
                    _lockers.Add(exchangeRateDate, new SemaphoreSlim(1, 1));
                }

                return _lockers[exchangeRateDate];
            }
        }

        public void ClearLockers()
        {
            lock (_lockers)
            {
                foreach (var locker in _lockers)
                {
                    locker.Value.Dispose();
                }
                _lockers.Clear();
            }
        }
    }
}
