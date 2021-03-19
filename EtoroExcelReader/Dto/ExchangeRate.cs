using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace EtoroExcelReader.Dto
{
    public class ExchangeRate
    {
        [JsonProperty(PropertyName = "number")]
        public string Number { get; set; }

        [JsonProperty(PropertyName = "effectiveDate")]
        public DateTime Date { get; set; }

        [JsonProperty(PropertyName = "mid")]
        public decimal Rate { get; set; }
    }
}
