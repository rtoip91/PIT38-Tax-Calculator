using Calculations.Dto;
using Calculations.Exceptions;
using Calculations.Extensions;
using Calculations.Interfaces;
using Database;
using Database.Entities;
using Newtonsoft.Json;
using System.Net;

namespace Calculations
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
                return await HandleBankHoliday(currencyCode, newDate);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        private async Task<ExchangeRateEntity> HandleBankHoliday(string currencyCode, DateTime holidayDate)
        {
            DateTime holidayDay = holidayDate;
            ExchangeRateEntity entity = await GetRateForPreviousDay(currencyCode, holidayDate);
            entity.Date = holidayDay;
            return await entity.MakeCopyAndSaveToDb();
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

                if(resp.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new BankHolidayException();
                }

                string result = await resp.Content.ReadAsStringAsync();
                ExchangeRates exchangeRates = new ExchangeRates();
                try
                {
                    exchangeRates = JsonConvert.DeserializeObject<ExchangeRates>(result);
                }
                catch (Exception ex)
                {
                    throw;
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