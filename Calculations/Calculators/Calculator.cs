using Calculations.Dto;
using Calculations.Interfaces;

namespace Calculations.Calculators
{
    internal class Calculator : ICalculator<CalculationResultDto>
    {      

        private readonly ICalculator<CfdCalculatorDto> _cfdCalculator;
        private readonly ICalculator<CryptoDto> _cryptoCalculator;
        private readonly ICalculator<StockCalculatorDto> _stockCalculator;
        private readonly ICalculator<DividendCalculatorDto> _dividendCalculator;

        public Calculator(ICalculator<CfdCalculatorDto> cfdCalculator,
            ICalculator<CryptoDto> cryptoCalculator,
            ICalculator<StockCalculatorDto> stockCalculator,
            ICalculator<DividendCalculatorDto> dividendCalculator)
        {
            _cfdCalculator = cfdCalculator;
            _cryptoCalculator = cryptoCalculator;
            _stockCalculator = stockCalculator;
            _dividendCalculator = dividendCalculator;
        }

        public async Task<T> Calculate<T>() where T : CalculationResultDto
        {
            var calculationResultDto = new CalculationResultDto();

            calculationResultDto.CdfDto = await _cfdCalculator.Calculate<CfdCalculatorDto>();          
            calculationResultDto.CryptoDto = await _cryptoCalculator.Calculate<CryptoDto>();          
            calculationResultDto.DividendDto = await _dividendCalculator.Calculate<DividendCalculatorDto>();          
            calculationResultDto.StockDto = await _stockCalculator.Calculate<StockCalculatorDto>();            
            return (T)calculationResultDto;
        }    
    }
}