using System;
using System.Globalization;

namespace ExcelReader.ExtensionMethods
{
    internal static class DataRowExtensions
    {
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