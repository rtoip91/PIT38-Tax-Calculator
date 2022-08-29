namespace Calculations.Dto
{
    public record CalculationResultDto
    {
        public CfdCalculatorDto CdfDto { get; set; }
        public CryptoDto CryptoDto { get; set; }
        public DividendCalculatorDto DividendDto { get; set; }
        public StockCalculatorDto StockDto { get; set; }
    }
}