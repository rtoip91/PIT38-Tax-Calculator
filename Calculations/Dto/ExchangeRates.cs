using System.Collections.Generic;
using Newtonsoft.Json;

namespace Calculations.Dto
{
    internal class ExchangeRates
    {
        [JsonProperty(PropertyName = "currency")]
        public string Currency { get; set; }
        
        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "rates")]
        public IList<ExchangeRate> Rates { get; set; }
    }
}
