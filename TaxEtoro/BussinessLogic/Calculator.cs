using System.Threading.Tasks;
using TaxEtoro.Interfaces;

namespace TaxEtoro.BussinessLogic
{
    internal class Calculator : ICalculator
    {
        private readonly ICfdCalculator _cfdCalculator;
        private readonly ICryptoCalculator _cryptoCalculator;
        public Calculator(ICfdCalculator cfdCalculator,
            ICryptoCalculator cryptoCalculator)
        {
            _cfdCalculator = cfdCalculator;
            _cryptoCalculator = cryptoCalculator;
        }

        public async Task Calculate()
        {
            await _cfdCalculator.Calculate();
            await _cryptoCalculator.Calculate();
        }
    }
}
