using Calculations.Dto;
using Calculations.Interfaces;
using Database.DataAccess.Interfaces;
using Database.Entities;

namespace Calculations.Calculators
{
    public class DividendCalculator : ICalculator<DividendCalculatorDto>
    {
        private readonly IExchangeRates _exchangeRates;
        private readonly IDividendsDataAccess _dividendsDataAccess;

        public DividendCalculator(IExchangeRates exchangeRates,
            IDividendsDataAccess dividendsDataAccess)
        {
            _exchangeRates = exchangeRates;
            _dividendsDataAccess = dividendsDataAccess;
        }

        public async Task<T> Calculate<T>() where T : DividendCalculatorDto
        {
            decimal sum = 0;
            var dividends = await _dividendsDataAccess.GetDividends();

            foreach (var dividend in dividends)
            {
                ExchangeRateEntity exchangeRateEntity =
                    await _exchangeRates.GetRateForPreviousDay("USD", dividend.DateOfPayment);
                decimal value = dividend.NetDividendReceived * exchangeRateEntity.Rate;
                sum += value;
            }

            sum = Math.Round(sum, 2);

            return (T)new DividendCalculatorDto { Dividend = sum };
        }
    }
}