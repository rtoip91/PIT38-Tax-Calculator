namespace Calculations.Extensions
{
    internal static class DecimalRoundingExtension
    {
        internal static decimal RoundDecimal(this decimal value, int decimals = 2)
        {
            return Math.Round(value, decimals, MidpointRounding.AwayFromZero);
        }
    }
}