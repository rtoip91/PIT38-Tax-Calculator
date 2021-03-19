using System.Collections.Generic;

namespace EtoroExcelReader.Dto
{
    internal static class Dictionaries
    {
        public static Dictionary<string, string> StockExchangesDictionary = new Dictionary<string, string>
        {
            {"ZU","Szwajcaria"},
            {"DE","Niemcy" },
            {"L", "Wielka Brytania" },
            {"PA", "Francja" },
            {"MC", "Hiszpania" },
            {"MI", "Włochy" },
            {"OL", "Norwegia" },
            {"ST", "Szwecja" },
            {"CO", "Dania" },
            {"HE", "Finlandia" },
            {"HK","Hong Kong" },
            {"LSB", "Portugalia" },
            {"BR", "Belgia" },
            {"NV", "Holandia" }
        };

        public static Dictionary<string, string> CryptoCurrenciesDictionary = new Dictionary<string, string>
        {
            {"BTC", "Bitcoin" },
            {"Ethereum","eth" }
        };
    }
}
