namespace Calculations.Dto
{
    public record StockCalculatorDto
    {
        public decimal Cost { get; set; }
        public decimal Revenue { get; set; }
        public decimal Income { get; set; }
    }
}