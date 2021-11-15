using Database;
using Database.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;
using TaxEtoro.BussinessLogic.Dto;
using TaxEtoro.Interfaces;

namespace TaxEtoro.BussinessLogic
{
    public class DividendCalculator : ICalculator<DividendCalculatorDto>
    {
        private readonly IExchangeRatesGetter _exchangeRatesGetter;

        public DividendCalculator(IExchangeRatesGetter exchangeRatesGetter)
        {
            _exchangeRatesGetter = exchangeRatesGetter;
        }

        public async Task<T> Calculate<T>() where T : DividendCalculatorDto
        {
            using (var context = new ApplicationDbContext())
            {
                decimal sum = 0;
                decimal originalSum = 0;
                var transReports = context.TransactionReports.Where(c => c.Details.ToLower().Contains("Payment caused by dividend".ToLower()) 
                                                                    || c.Details.ToLower().Contains("Płatność w wyniku dywidendy".ToLower()));
                int temp = transReports.Count();

                foreach (var transaction in transReports)
                {
                    ExchangeRateEntity exchangeRateEntity = await _exchangeRatesGetter.GetRateForPreviousDay("USD", transaction.Date);
                    decimal value = transaction.Amount * exchangeRateEntity.Rate;
                    sum += value;
                    originalSum += transaction.Amount;
                }

                sum = Math.Round(sum, 2);

                return new DividendCalculatorDto { Dividend = sum } as T;               
            }
        }
    }
}
