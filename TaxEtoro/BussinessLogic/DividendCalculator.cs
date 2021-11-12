using Database;
using Database.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;
using TaxEtoro.Interfaces;

namespace TaxEtoro.BussinessLogic
{
    internal class DividendCalculator : ICalculator
    {
        private readonly IExchangeRatesGetter _exchangeRatesGetter;

        public DividendCalculator(IExchangeRatesGetter exchangeRatesGetter)
        {
            _exchangeRatesGetter = exchangeRatesGetter;
        }


        public async Task Calculate()
        {
            using (var context = new ApplicationDbContext())
            {
                decimal sum = 0;
                decimal originalSum = 0;
                var transReports = context.TransactionReports.Where(c => c.Details.ToLower().Contains("Payment caused by dividend".ToLower()));
                int temp = transReports.Count();

                foreach (var transaction in transReports)
                {
                    ExchangeRateEntity exchangeRateEntity = await _exchangeRatesGetter.GetRateForPreviousDay("USD", transaction.Date);
                    decimal value = transaction.Amount * exchangeRateEntity.Rate;
                    sum += value;
                    originalSum += transaction.Amount;
                }

                Console.WriteLine("Dywidendy:");
                Console.WriteLine($"Suma dywidend = {sum}");
                Console.WriteLine();
            }

        }
    }
}
