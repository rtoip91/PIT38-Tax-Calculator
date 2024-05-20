namespace Calculations.Extensions
{
    /// <summary>
    /// Provides extension methods for rounding decimal values.
    /// </summary>
    internal static class DecimalRoundingExtension
    {
        /// <summary>
        /// Rounds a decimal value to a specified number of fractional digits, using the specified rounding convention.
        /// </summary>
        /// <param name="value">The decimal number to be rounded.</param>
        /// <param name="decimals">The number of fractional digits in the return value. Default is 2.</param>
        /// <returns>The number nearest to the value parameter that contains a number of fractional digits equal to the decimals parameter.</returns>
        internal static decimal RoundDecimal(this decimal value, int decimals = 2)
        {
            return Math.Round(value, decimals, MidpointRounding.AwayFromZero);
        }
    }
}