using System.Threading.Tasks;
using TaxEtoro.Interfaces;

namespace TaxEtoro.BussinessLogic
{
    internal class Calculator : ICalculator
    {
        private readonly ICfdCalculator _cfdCalculator;
        private readonly ICryptoCalculator _cryptoCalculator;
        private readonly IStockCalculator _stockCalculator;
        private readonly IDividendCalculator _dividendCalculator;

        public Calculator(ICfdCalculator cfdCalculator,
            ICryptoCalculator cryptoCalculator,
            IStockCalculator stockCalculator,
            IDividendCalculator dividendCalculator)
        {
            _cfdCalculator = cfdCalculator;
            _cryptoCalculator = cryptoCalculator;
            _stockCalculator = stockCalculator;
            _dividendCalculator = dividendCalculator;
        }

        public async Task Calculate()
        {
            await _cfdCalculator.Calculate();
            await _cryptoCalculator.Calculate();
            await _dividendCalculator.CalculateDividend();
            await _stockCalculator.Calculate();
        }
    }
}
