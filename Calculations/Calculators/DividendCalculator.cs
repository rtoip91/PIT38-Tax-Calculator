using System.Globalization;
using Calculations.Dto;
using Calculations.Interfaces;
using Database.DataAccess.Interfaces;
using Database.Entities;

namespace Calculations.Calculators
{
    public class DividendCalculator : ICalculator<DividendCalculatorDto>
    {
        private readonly IExchangeRates _exchangeRates;
        private readonly IDividendsDataAccess _dividendsDataAccess;
        private readonly IDividendCalculationsDataAccess _dividendCalculationsDataAccess;

        public DividendCalculator(IExchangeRates exchangeRates,
            IDividendsDataAccess dividendsDataAccess,
            IDividendCalculationsDataAccess dividendCalculationsDataAccess)
        {
            _exchangeRates = exchangeRates;
            _dividendsDataAccess = dividendsDataAccess;
            _dividendCalculationsDataAccess = dividendCalculationsDataAccess;
        }

        public async Task<T> Calculate<T>() where T : DividendCalculatorDto
        {
            decimal sum = 0;
            decimal taxPaid = 0;
            decimal taxToBePaid = 0;

            var dividends = _dividendsDataAccess.GetDividends();
            List<DividendCalculationsEntity> dividendCalculationsEntities = new List<DividendCalculationsEntity>();

            foreach (var dividend in dividends)
            {
                DividendCalculationsEntity dividendCalculations = new DividendCalculationsEntity
                {
                    Currency = "USD",
                    DateOfPayment = dividend.DateOfPayment,
                    InstrumentName = dividend.InstrumentName,
                    DividendReceived = dividend.NetDividendReceived + dividend.WithholdingTaxAmount,
                    PositionId = dividend.PositionId
                };
                RegionInfo regionInfo = new RegionInfo(dividend.ISIN);

                dividendCalculations.Country = regionInfo.EnglishName;

                ExchangeRateEntity exchangeRateEntity = await _exchangeRates.GetRateForPreviousDay(dividendCalculations.Currency, dividend.DateOfPayment);
                dividendCalculations.ExchangeRate = exchangeRateEntity.Rate;
                dividendCalculations.ExchangeRateDate = exchangeRateEntity.Date;

                dividendCalculations.DividendReceivedExchanged = Math.Round(dividendCalculations.DividendReceived * dividendCalculations.ExchangeRate, 2); 

                dividendCalculations.WithholdingTaxRate = dividend.ISIN.Equals("US") ? 15 : dividend.WithholdingTaxRate;

                dividendCalculations.WithholdingTaxPaid = Math.Round(dividendCalculations.DividendReceivedExchanged * dividendCalculations.WithholdingTaxRate / 100 , 2);

                decimal tax19percent = Math.Round(dividendCalculations.DividendReceivedExchanged * 0.19m, 2);

                decimal totalToBePaid = tax19percent - dividendCalculations.WithholdingTaxPaid;

                dividendCalculations.WithholdingTaxRemain = totalToBePaid > 0 ? totalToBePaid : 0;

                sum += dividendCalculations.DividendReceivedExchanged;

                taxPaid += TaxPaid(dividendCalculations, tax19percent);

                taxToBePaid += dividendCalculations.WithholdingTaxRemain;

                dividendCalculationsEntities.Add(dividendCalculations);
            }

            await _dividendCalculationsDataAccess.AddEntities(dividendCalculationsEntities);

            sum = Math.Round(sum, 2);

            return (T)new DividendCalculatorDto
            {
                Dividend = sum,
                TaxPaid = taxPaid,
                TaxToBePaid = taxToBePaid
            };
        }

        private decimal TaxPaid(DividendCalculationsEntity dividendCalculations, decimal tax19percent)
        {
            decimal taxPaid;
            if (dividendCalculations.WithholdingTaxPaid <= tax19percent)
            {
                taxPaid = dividendCalculations.WithholdingTaxPaid;
            }
            else
            {
                taxPaid = tax19percent;
            }

            return taxPaid;
        }
    }
}