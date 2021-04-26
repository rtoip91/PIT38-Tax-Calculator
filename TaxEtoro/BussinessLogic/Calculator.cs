using System.Threading.Tasks;
using TaxEtoro.Interfaces;

namespace TaxEtoro.BussinessLogic
{
    internal class Calculator : ICalculator
    {
        private readonly ICfdCalculator _cfdCalculator;
        private readonly ICryptoCalculator _cryptoCalculator;
        private readonly IStockCalculator _stockCalculator;

        public Calculator(ICfdCalculator cfdCalculator,
            ICryptoCalculator cryptoCalculator,
            IStockCalculator stockCalculator)
        {
            _cfdCalculator = cfdCalculator;
            _cryptoCalculator = cryptoCalculator;
            _stockCalculator = stockCalculator;
        }

        public async Task Calculate()
        {
            await _cfdCalculator.Calculate();           
            await _cryptoCalculator.Calculate();
            await _stockCalculator.Calculate();
        }
    }
}
