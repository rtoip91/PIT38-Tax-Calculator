namespace Calculations.Interfaces
{
    public interface IExchangeRatesLocker
    {
        SemaphoreSlim GetLocker(DateTime exchangeRateDate);
        void ClearLockers();
    }
}
