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
                return entity;
            }
            catch (BankHolidayException)
            {                
                entity = await GetRateForPreviousDay(currencyCode, newDate);
                return entity;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        private async Task<ExchangeRateEntity> GetRateForDay(string currencyCode, DateTime date)
        {
            using (var context = new ApplicationDbContext())
            {
                ExchangeRateEntity exchangeRate = context.ExchangeRates.FirstOrDefault(rate => rate.Code == currencyCode && rate.Date.Date == date.Date);

                if (exchangeRate != null)
                {
                    return exchangeRate;
                }

                exchangeRate = await GetRatesFromApi(currencyCode, date);
                await context.AddAsync(exchangeRate);
                await context.SaveChangesAsync();              

                return exchangeRate;
            }
        }      

        private async Task<ExchangeRateEntity> GetRatesFromApi(string currencyCode, DateTime date)
        {
            using (HttpClient client = new HttpClient())
            {
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
                    // There will be no rates when this day is bank holiday
                    throw new BankHolidayException();
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
}