using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Database;
using Database.Entities;
using Newtonsoft.Json;
using TaxEtoro.BussinessLogic.Dto;
using TaxEtoro.Interfaces;

namespace TaxEtoro.BussinessLogic
{
    public class ExchangeRatesGetter : IExchangeRatesGetter
    {

        public async Task<ExchangeRateEntity> GetRateForPreviousDay(string currencyCode, DateTime date)
        {
            double subtractDays = -1;
            
            if(date.DayOfWeek == DayOfWeek.Sunday)
            {
                subtractDays = -2;
            }
            if(date.DayOfWeek == DayOfWeek.Monday)
            {
                subtractDays = -3;
            }

            DateTime newDate = date.AddDays(subtractDays);
            ExchangeRateEntity entity;
            try
            {
                entity = await GetRateForDay(currencyCode, newDate);
            }
            catch
            {
                entity = await GetRateForPreviousDay(currencyCode, newDate);
            }

            return entity;
        }

        private async Task<ExchangeRateEntity> GetRateForDay(string currencyCode, DateTime date)
        {
            using (var context = new ApplicationDbContext())
            {
                ExchangeRateEntity exchangeRate = context.ExchangeRates.FirstOrDefault(rate => rate.Code == currencyCode && rate.Date.Date == date.Date);

                if (exchangeRate == null)
                {
                    exchangeRate = await GetRatesFromApi(currencyCode, date);
                    await context.AddAsync(exchangeRate);
                    await context.SaveChangesAsync();
                    Console.WriteLine($"Pobrano z API dla dnia {exchangeRate.Date} kurs wynosił {exchangeRate.Rate}");
                    await Task.Delay(1000);                   
                }
                else
                {
                    Console.WriteLine($"Wczytano z bazy dla dnia {exchangeRate.Date} kurs wynosił {exchangeRate.Rate}");
                }
           
                return exchangeRate;
            }
        }      

        private async Task<ExchangeRateEntity> GetRatesFromApi (string currencyCode, DateTime date)
        {
            HttpClient client = new HttpClient();
            string url = $"http://api.nbp.pl/api/exchangerates/rates/a/{currencyCode.ToLower()}/{date:yyyy-MM-dd}/";
            var resp = await client.GetAsync(url);
            string result = await resp.Content.ReadAsStringAsync();
            ExchangeRates exchangeRates = new ExchangeRates();
            try
            {
                exchangeRates = JsonConvert.DeserializeObject<ExchangeRates>(result);
            }
            catch
            {
                Console.WriteLine($"Nie udało się pobrać danych z api dla waluty {currencyCode} i daty {date:yyyy-MM-dd}");
                throw new Exception("Bank Holiday Exception");
            }

            ExchangeRateEntity entity = new ExchangeRateEntity();

            entity.Code = currencyCode;
            entity.Currency = exchangeRates.Currency;

            var tempRate = exchangeRates.Rates.FirstOrDefault();

            if (tempRate != null)
            {
                entity.Date = tempRate.Date;
                entity.Rate = tempRate.Rate;
            }

            return entity;
        }

        
    }
}