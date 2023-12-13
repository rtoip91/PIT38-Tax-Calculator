using Calculations.Dto;
using Calculations.Exceptions;
using Calculations.Interfaces;
using Newtonsoft.Json;
using System.Net;
using Database.DataAccess.Interfaces;
using Database.Entities.Database;
using Microsoft.Extensions.Logging;
using Polly;

namespace Calculations
{
    internal class ExchangeRates : IExchangeRates
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IExchangeRatesDataAccess _exchangeRatesDataAccess;
        private readonly ILogger<ExchangeRates> _logger;

        public ExchangeRates(IHttpClientFactory httpClientFactory,
            IExchangeRatesDataAccess exchangeRatesDataAccess,
            ILogger<ExchangeRates> logger)
        {
            _httpClientFactory = httpClientFactory;
            _exchangeRatesDataAccess = exchangeRatesDataAccess;
            _logger = logger;
        }

        private async Task<ExchangeRateEntity> HandleGetRateForPreviousDay(string currencyCode, DateTime date,
            bool bankHoliday = false)
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

            try
            {
                ExchangeRateEntity entity = await GetRateForDay(currencyCode, newDate);
                return entity;
            }
            catch (BankHolidayException)
            {
                _logger.LogInformation($"Bank holiday on {newDate.ToShortDateString()}");
                return await HandleBankHoliday(currencyCode, newDate);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<ExchangeRateEntity> GetRateForPreviousDay(string currencyCode, DateTime date)
        {
            return await HandleGetRateForPreviousDay(currencyCode, date);
        }

        private async Task<ExchangeRateEntity> HandleBankHoliday(string currencyCode, DateTime holidayDate)
        {
            ExchangeRateEntity entity = await HandleGetRateForPreviousDay(currencyCode, holidayDate, true);
            entity.Date = holidayDate;
            return await _exchangeRatesDataAccess.MakeCopyAndSaveToDb(entity);
        }

        private async Task<ExchangeRateEntity> GetRateForDay(string currencyCode, DateTime date)
        {
            ExchangeRateEntity exchangeRate = await _exchangeRatesDataAccess.GetRate(currencyCode, date);

            if (exchangeRate != null)
            {
                return exchangeRate;
            }

            _logger.LogInformation($"Getting exchange rate from api {currencyCode} [{date.ToShortDateString()}] ");
            exchangeRate = await GetRatesFromApi(currencyCode, date);
            await _exchangeRatesDataAccess.SaveRate(exchangeRate);
            return exchangeRate;
        }

        private async Task<ExchangeRateEntity> GetRatesFromApi(string currencyCode, DateTime date)
        {
            var policy = Policy.Handle<HttpRequestException>()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
            
            string result = await policy.ExecuteAsync(() => GetResultFromNbpApi(currencyCode, date));

            ExchangeRatesDto? exchangeRates;
            try
            {
                exchangeRates = JsonConvert.DeserializeObject<ExchangeRatesDto>(result);
            }
            catch (JsonReaderException)
            {
                _logger.LogError("Could not parse given exchange rate response");
                _logger.LogInformation(result);
                throw;
            }

            ExchangeRateEntity entity = new ExchangeRateEntity
            {
                Code = currencyCode
            };

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

            _logger.LogInformation(
                $"Successfully received exchange rate from api {entity.Code} [{entity.Date.ToShortDateString()}]");
            return entity;
        }

        private async Task<string> GetResultFromNbpApi(string currencyCode, DateTime date)
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
                throw new HttpRequestException("NBP server is not responding");
            }

            if (resp.StatusCode == HttpStatusCode.TooManyRequests)
            {
                throw new HttpRequestException();
            }

            return await resp.Content.ReadAsStringAsync();
        }
    }
}