using Calculations.Dto;
using Calculations.Interfaces;

namespace Calculations.Calculators
{
    internal class Calculator : ICalculator<CalculationResultDto>, ICalculationEvents
    {
        public event EventHandler? CfdCalculationFinished;
        public event EventHandler? CryptoCalculationFinished;
        public event EventHandler? DividendCalculationFinished;
        public event EventHandler? StockCalculationFinished;

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
            OnCfdCalculationFinished();
            calculationResultDto.CryptoDto = await _cryptoCalculator.Calculate<CryptoDto>();
            OnCryptoCalculationFinished();
            calculationResultDto.DividendDto = await _dividendCalculator.Calculate<DividendCalculatorDto>();
            OnDividendCalculationFinished();
            calculationResultDto.StockDto = await _stockCalculator.Calculate<StockCalculatorDto>();
            OnStockCalculationFinished();
            return (T)calculationResultDto;
        }

        private void OnCfdCalculationFinished()
        {
            CfdCalculationFinished?.Invoke(this, null);
        }

        private void OnCryptoCalculationFinished()
        {
            if (CryptoCalculationFinished != null)
            {
                CryptoCalculationFinished(this, null);
            }
        }

        private void OnDividendCalculationFinished()
        {
            if (DividendCalculationFinished != null)
            {
                DividendCalculationFinished(this, null);
            }
        }

        private void OnStockCalculationFinished()
        {
            if (StockCalculationFinished != null)
            {
                StockCalculationFinished(this, null);
            }
        }
    }
}