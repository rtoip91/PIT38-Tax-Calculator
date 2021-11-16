using System;

namespace TaxEtoro.Interfaces
{
    internal interface ICalculationEvents
    {
        event EventHandler CfdCalculationFinished;
        event EventHandler CryptoCalculationFinished;
        event EventHandler DividendCalculationFinished;
        event EventHandler StockCalculationFinished;
    }
}
