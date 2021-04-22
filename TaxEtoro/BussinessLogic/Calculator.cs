using System.Threading.Tasks;

namespace TaxEtoro.BussinessLogic
{
    internal class Calculator : ICalculator
    {
        private readonly ICfdCalculator _cfdCalculator;

        public Calculator(ICfdCalculator cfdCalculator)
        {
            _cfdCalculator = cfdCalculator;
        }

        public async Task Calculate()
        {
            await _cfdCalculator.Calculate();
        }
    }
}
