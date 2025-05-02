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
            if (item is null)
                return 0;

            ReadOnlySpan<char> value = item.ToString();
            if (value.IsEmpty || value.IsWhiteSpace())
            {
                return 0;
            }

            if (value.EndsWith('%'))
            {
                value = value.Slice(0, value.Length - 1);
            }

            return decimal.Parse(value, ChooseProvider(value));
        }

        internal static string ToCountryName(this object item)
        {
            if (item is null)
                return CyprusIsoCode;

            ReadOnlySpan<char> value = item.ToString();
            if (value.IsEmpty)
            {
                return CyprusIsoCode;
            }

            string isoString = value.Slice(0, 2).ToString();
            try
            {
                Country countryData = Country.List.First(c => c.TwoLetterCode == isoString);
                return countryData.Name;
            }
            catch (Exception)
            {
                Console.WriteLine($"Unsupported code {isoString}");
                throw;
            }
        }

        internal static TransactionType ToTransactionType(this object item)
        {
            if (item is null)
                return TransactionType.Long;

            ReadOnlySpan<char> value = item.ToString();
            if (value.IsEmpty) return TransactionType.Long;

            if (value.StartsWith("Sprzedaj", StringComparison.Ordinal))
            {
                return TransactionType.Short;
            }

            if (value.StartsWith("Sell", StringComparison.Ordinal))
            {
                return TransactionType.Short;
            }
            
            if (value.StartsWith("Long", StringComparison.Ordinal))
            {
                return TransactionType.Long;
            }
            
            if (value.StartsWith("Short", StringComparison.Ordinal))
            {
                return TransactionType.Short;
            }
            

            return TransactionType.Long;
        }

        internal static string OperationToString(this object item)
        {
            if (item is null)
                return string.Empty;

            string value = item.ToString();
            var span = value.AsSpan();

            if (span.StartsWith("Buy", StringComparison.Ordinal))
            {
                value = "Kup" + value.Substring(3);
            }
            else if (span.StartsWith("Sell", StringComparison.Ordinal))
            {
                value = "Sprzedaj" + value.Substring(4);
            }

            return value;
        }

        internal static int ToInt(this object item)
        {
            if (item is null)
                return 0;

            ReadOnlySpan<char> value = item.ToString();
            if (value.IsEmpty || value.IsWhiteSpace())
            {
                return 0;
            }

            return int.Parse(value, ChooseProvider(value));
        }

        internal static long ToLong(this object item)
        {
            if (item is null)
                return 0;

            ReadOnlySpan<char> value = item.ToString();
            if (value.IsEmpty || value.IsWhiteSpace())
            {
                return 0;
            }

            return long.TryParse(value, out var result) ? result : 0;
        }

        internal static DateTime ToDate(this object item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            string value = item.ToString();
            return DateTime.Parse(value, new CultureInfo("pl-PL"));
        }

        private static CultureInfo ChooseProvider(ReadOnlySpan<char> value)
        {
            return value.Contains('.') ? CultureInfo.InvariantCulture : new CultureInfo("pl-PL");
        }
    }
}
