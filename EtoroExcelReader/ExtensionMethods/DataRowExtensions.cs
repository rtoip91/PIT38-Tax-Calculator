using System;
using System.Globalization;

namespace ExcelReader.ExtensionMethods
{
    public static class DataRowExtensions

    {
        public static decimal ToDecimal(this object item)
        {
            string value = item.ToString();
            if (string.IsNullOrWhiteSpace(value))
            {
                return 0;
            }

            return decimal.Parse(value, ChooseProvider(value));
        }

        public static int ToInt(this object item)
        {
            string value = item.ToString();
            if(string.IsNullOrWhiteSpace(value))
            {
                return 0;
            }

            return int.Parse(value, ChooseProvider(value));
        }

        public static DateTime ToDate(this object item)
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
