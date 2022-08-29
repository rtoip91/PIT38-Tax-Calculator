namespace Calculations.Dto
{
    public record DividendCalculatorDto
    {
        public decimal Dividend { get; set; }
        public decimal TaxPaid { get; set; }

        public decimal TaxToBePaid { get; set; }
    }
}