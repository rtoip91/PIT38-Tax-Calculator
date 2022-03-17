using System;
using System.Globalization;
using Database.Enums;

namespace ExcelReader.ExtensionMethods
{
    internal static class DataRowExtensions
    {
        private const string CyprusIsoCode = "CY";

        internal static decimal ToDecimal(this object item)
        {
            string value = item.ToString();
            if (string.IsNullOrWhiteSpace(value))
            {
                return 0;
            }

            value = value.TrimEnd('%');
            return decimal.Parse(value, ChooseProvider(value));
        }

        internal static string ToIso3166Symbol(this object item)
        {
            string value = item.ToString();
            return string.IsNullOrWhiteSpace(value) ? CyprusIsoCode : value.Substring(0, 2);
        }

        internal static TransactionType ToTransactionType(this object item)
        {
            string value = item.ToString();
            if (value != null && value.Substring(0, 4).Contains("Sell"))
            {
                return TransactionType.Short;
            }

            return TransactionType.Long;
        }


        internal static string OperationToString(this object item)
        {
            string value = item.ToString();

            if (value != null && value.Substring(0,4).Contains("Buy"))
            {
                value = value.Replace("Buy", "Kupno");
            }
            if (value != null && value.Substring(0,4).Contains("Sell"))
            {
                value = value.Replace("Sell", "Sprzedaż");
            }

            return value;
        }

        internal static int ToInt(this object item)
        {
            string value = item.ToString();
            if (string.IsNullOrWhiteSpace(value))
            {
                return 0;
            }

            return int.Parse(value, ChooseProvider(value));
        }

        internal static DateTime ToDate(this object item)
        {
            string value = item.ToString();
            return DateTime.Parse(value);
        }

        private static CultureInfo ChooseProvider(string value)
        {
            if (value.Contains('.'))
            {
                return CultureInfo.InvariantCulture;
            }
            else
            {
                return new CultureInfo("pl-PL");
            }
        }
    }
}