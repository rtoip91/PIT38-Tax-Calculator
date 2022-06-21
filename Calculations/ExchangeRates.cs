using Calculations.Dto;
using Calculations.Exceptions;
using Calculations.Interfaces;
using Database.Entities;
using Newtonsoft.Json;
using System.Net;
using Database.DataAccess.Interfaces;

namespace Calculations
{
    internal class ExchangeRates : IExchangeRates
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IExchangeRatesDataAccess _exchangeRatesDataAccess;

        public ExchangeRates(IHttpClientFactory httpClientFactory, IExchangeRatesDataAccess exchangeRatesDataAccess)
        {
            _httpClientFactory = httpClientFactory;
            _exchangeRatesDataAccess = exchangeRatesDataAccess;
        }

        public async Task<ExchangeRateEntity> GetRateForPreviousDay(string currencyCode, DateTime date)
        {
            double subtractDays = -1;

            if (date.DayOfWeek == DayOfWeek.Sunday)
            {
                subtractDays = -2;
            }

            if (date.DayOfWeek == DayOfWeek.Monday)
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private async Task<ExchangeRateEntity> HandleBankHoliday(string currencyCode, DateTime holidayDate)
        {
            DateTime holidayDay = holidayDate;
            ExchangeRateEntity entity = await GetRateForPreviousDay(currencyCode, holidayDate);
            entity.Date = holidayDay;
            return await _exchangeRatesDataAccess.MakeCopyAndSaveToDb(entity);
        }

        private async Task<ExchangeRateEntity> GetRateForDay(string currencyCode, DateTime date)
        {
            ExchangeRateEntity exchangeRate = await _exchangeRatesDataAccess.GetRate(currencyCode, date);

            if (exchangeRate != null)
            {
                return exchangeRate;
            }

            exchangeRate = await GetRatesFromApi(currencyCode, date);
            await _exchangeRatesDataAccess.SaveRate(exchangeRate);
            return exchangeRate;
        }

        private async Task<ExchangeRateEntity> GetRatesFromApi(string currencyCode, DateTime date)
        {
            var httpClient = _httpClientFactory.CreateClient("ExchangeRates");
            using var resp = await httpClient.GetAsync($"{currencyCode.ToLower()}/{date:yyyy-MM-dd}/",
                HttpCompletionOption.ResponseHeadersRead);
            if (resp.StatusCode == HttpStatusCode.NotFound)
            {
                throw new BankHolidayException();
            }

            if (resp.StatusCode == HttpStatusCode.ServiceUnavailable)
            {
                throw new Exception("Serwer NBP nie odpowiada");
            }

            string result = await resp.Content.ReadAsStringAsync();
            ExchangeRatesDto? exchangeRates;
            try
            {
                exchangeRates = JsonConvert.DeserializeObject<ExchangeRatesDto>(result);
            }
            catch (JsonReaderException)
            {
                Console.WriteLine("Could not parse given exchange rate response");
                Console.WriteLine(result);
                throw;
            }
            
            ExchangeRateEntity entity = new ExchangeRateEntity();

            entity.Code = currencyCode;
            if (exchangeRates != null)
            {
                entity.Currency = exchangeRates.Currency;
                var tempRate = exchangeRates.Rates.FirstOrDefault();
                if (tempRate != null)
                {
                    entity.Date = tempRate.Date;
                    entity.Rate = tempRate.Rate;
                }
            }

            return entity;
        }
    }
}