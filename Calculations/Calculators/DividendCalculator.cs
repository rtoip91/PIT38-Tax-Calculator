using Calculations.Dto;
using Calculations.Interfaces;
using Database;
using Database.DataAccess.Interfaces;
using Database.Entities;

namespace Calculations.Calculators
{
    public class DividendCalculator : ICalculator<DividendCalculatorDto>
    {
        private readonly IExchangeRates _exchangeRates;
        private readonly ITransactionReportsDataAccess _transactionReportsDataAccess;

        public DividendCalculator(IExchangeRates exchangeRates,
            ITransactionReportsDataAccess transactionReportsDataAccess)
        {
            _exchangeRates = exchangeRates;
            _transactionReportsDataAccess = transactionReportsDataAccess;
        }

        public async Task<T> Calculate<T>() where T : DividendCalculatorDto
        {
            decimal sum = 0;
            var transReports = await _transactionReportsDataAccess.GetDividendTransactions();

            foreach (var transaction in transReports)
            {
                ExchangeRateEntity exchangeRateEntity =
                    await _exchangeRates.GetRateForPreviousDay("USD", transaction.Date);
                decimal value = transaction.Amount * exchangeRateEntity.Rate;
                sum += value;
            }

            sum = Math.Round(sum, 2);

            return (T)new DividendCalculatorDto { Dividend = sum };
        }
    }
}