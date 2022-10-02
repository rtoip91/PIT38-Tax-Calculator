namespace Database.Entities.InMemory
{
    public record IncomeByCountryEntity
    {
        public string Country { get; set; }
        public decimal Income { get; set; }
        public decimal PaidTax { get; set; }

        public override string ToString()
        {
            return $"\nKraj: {Country} | Dochód: {Income} PLN | Zapłacony podatek: {PaidTax} PLN";
        }
    }
}