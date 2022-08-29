namespace Calculations.Dto
{
    public record CryptoDto
    {
        public decimal Cost { get; set; }
        public decimal Revenue { get; set; }
        public decimal Income { get; set; }
        public decimal UnsoldCryptos { get; set; }
    }
}