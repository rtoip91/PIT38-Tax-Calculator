using System;
using System.Net.Http;
using System.Threading.Tasks;
using EtoroExcelReader.Dto;
using Newtonsoft.Json;

namespace EtoroExcelReader.Helpers
{
    internal class ExchangeRatesGetter
    {
        public async Task<ExchangeRates> GetRateForDay(string currencyCode, DateTime date)
        {
            HttpClient client = new HttpClient();
            string url = $"http://api.nbp.pl/api/exchangerates/rates/a/{currencyCode.ToLower()}/{date:yyyy-MM-dd}/";
            var resp = await client.GetAsync(url);
            string result = await resp.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ExchangeRates>(result);
        }
    }
}
