using Newtonsoft.Json;

namespace Calculations.Dto
{
    internal class ExchangeRatesDto
    {
        [JsonProperty(PropertyName = "currency")]
        public string Currency { get; set; }
        
        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "rates")]
        public IList<ExchangeRateDto> Rates { get; set; }
    }
}
