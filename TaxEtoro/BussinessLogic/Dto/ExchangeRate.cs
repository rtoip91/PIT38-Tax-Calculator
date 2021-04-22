using System;
using Newtonsoft.Json;

namespace TaxEtoro.BussinessLogic.Dto
{
    internal  class ExchangeRate
    {
        [JsonProperty(PropertyName = "number")]
        public string Number { get; set; }

        [JsonProperty(PropertyName = "effectiveDate")]
        public DateTime Date { get; set; }

        [JsonProperty(PropertyName = "mid")]
        public decimal Rate { get; set; }
    }
}
