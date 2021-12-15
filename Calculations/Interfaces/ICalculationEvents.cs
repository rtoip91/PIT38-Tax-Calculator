namespace Calculations.Interfaces
{
    public interface ICalculationEvents
    {
        event EventHandler? CfdCalculationFinished;
        event EventHandler? CryptoCalculationFinished;
        event EventHandler? DividendCalculationFinished;
        event EventHandler? StockCalculationFinished;
    }
}