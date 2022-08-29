using Newtonsoft.Json;

namespace Calculations.Dto
{
    internal record ExchangeRatesDto
    {
        [JsonProperty(PropertyName = "currency")]
        public string Currency { get; set; }

        [JsonProperty(PropertyName = "code")] public string Code { get; set; }

        [JsonProperty(PropertyName = "rates")] public IList<ExchangeRateDto> Rates { get; set; }
    }
}