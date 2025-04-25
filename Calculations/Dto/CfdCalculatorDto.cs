namespace Calculations.Dto
{
    public record CfdCalculatorDto
    {
        public decimal Gain { get; set; }
        public decimal Loss { get; set; }
        public decimal Income { get; set; }
    }
}