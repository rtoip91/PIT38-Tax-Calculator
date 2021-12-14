using Newtonsoft.Json;

namespace Calculations.Dto
{
    internal  class ExchangeRateDto
    {
        [JsonProperty(PropertyName = "number")]
        public string Number { get; set; }

        [JsonProperty(PropertyName = "effectiveDate")]
        public DateTime Date { get; set; }

        [JsonProperty(PropertyName = "mid")]
        public decimal Rate { get; set; }
    }
}
