namespace Calculations.Interfaces
{
    public interface IExchangeRatesLocker
    {
        SemaphoreSlim GetLocker(DateTime exchangeRateDate);
    }
}
