using System;
using System.Threading.Tasks;
using Database.Entities;

namespace Database.DataAccess.Interfaces
{
    public interface IExchangeRatesDataAccess
    {
        Task<ExchangeRateEntity> GetRate(string currencyCode, DateTime date);
        Task<int> SaveRate(ExchangeRateEntity exchangeRate);
        Task<ExchangeRateEntity> MakeCopyAndSaveToDb(ExchangeRateEntity item);
    }
}