using System;
using System.Globalization;
using System.Linq;
using Database.Enums;
using ExcelReader.Converters;

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

        internal static string ToCountryName(this object item)
        {
            var value = item.ToString();
            if (string.IsNullOrEmpty(value))
            {
                return CyprusIsoCode;
            }

            var isoString = value.Substring(0, 2);
            try
            {
                Country countryData = Country.List.First(c => c.TwoLetterCode == isoString);
                return countryData.Name;

            }
            catch (Exception e)
            {
                Console.WriteLine($"Unsupported code {isoString}",isoString);
                throw;
            }
           
        }

        internal static TransactionType ToTransactionType(this object item)
        {
            var value = item.ToString().AsSpan();
            if (value != null)
            {
                if (value.Length >= 8 && value.Slice(0, 8).Equals("Sprzedaj", StringComparison.Ordinal))
                {
                    return TransactionType.Short;
                }

                if (value.Length >= 4 && value.Slice(0, 4).Equals("Sell", StringComparison.Ordinal))
                {
                    return TransactionType.Short;
                }
            }

            return TransactionType.Long;
        }

        internal static string OperationToString(this object item)
        {
            string value = item.ToString();

            if (value.AsSpan(0, 3).Equals("Buy", StringComparison.Ordinal))
            {
                value = value.Replace("Buy", "Kup");
            }

            if (value.AsSpan(0, 4).Equals("Sell", StringComparison.Ordinal))
            {
                value = value.Replace("Sell", "Sprzedaj");
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

        internal static long ToLong(this object item)
        {
            string value = item.ToString();
            if (string.IsNullOrWhiteSpace(value))
            {
                return 0;
            }

            return long.Parse(value, ChooseProvider(value));
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