using System.Threading.Tasks;
using TaxEtoro.Interfaces;

namespace TaxEtoro.BussinessLogic
{
    internal class Calculator : ICalculator
    {
        private readonly ICalculator _cfdCalculator;
        private readonly ICalculator _cryptoCalculator;
        private readonly ICalculator _stockCalculator;
        private readonly ICalculator _dividendCalculator;
        private readonly IExchangeRatesGetter _exchangeRatesGetter;

        public Calculator(IExchangeRatesGetter exchangeRatesGetter)
        {
            _exchangeRatesGetter = exchangeRatesGetter;
            _cfdCalculator = new CfdCalculator(exchangeRatesGetter);
            _cryptoCalculator = new CryptoCalculator(exchangeRatesGetter);
            _stockCalculator = new StockCalculator(exchangeRatesGetter);
            _dividendCalculator = new DividendCalculator(exchangeRatesGetter);
        }

        public async Task Calculate()
        {
            await _cfdCalculator.Calculate();
            await _cryptoCalculator.Calculate();
            await _dividendCalculator.Calculate();
            await _stockCalculator.Calculate();
        }
    }
}
