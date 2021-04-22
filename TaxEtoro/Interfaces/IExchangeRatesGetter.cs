using Database.Entities;
using System;
using System.Threading.Tasks;

namespace TaxEtoro.Interfaces
{
    public interface IExchangeRatesGetter
    {
        Task<ExchangeRateEntity> GetRateForPreviousDay(string currencyCode, DateTime date);
    }
}
