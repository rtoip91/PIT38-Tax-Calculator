namespace Calculations.Extensions
{
    internal static class DecimalRoundingExtension
    {
        internal static decimal RoundDecimal(this decimal value)
        {
            return Math.Round(value, 2, MidpointRounding.AwayFromZero);
        }
    }
}
