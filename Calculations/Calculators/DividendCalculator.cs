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

            var dividends = await _dividendsDataAccess.GetDividends();
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
                RegionInfo regionInfo = new RegionInfo(dividend.ISIN.Substring(0, 2));

                dividendCalculations.Country = regionInfo.EnglishName;

                ExchangeRateEntity exchangeRateEntity = await _exchangeRates.GetRateForPreviousDay(dividendCalculations.Currency, dividend.DateOfPayment);
                dividendCalculations.ExchangeRate = exchangeRateEntity.Rate;

                dividendCalculations.DividendReceivedExchanged = Math.Round(dividendCalculations.DividendReceived * dividendCalculations.ExchangeRate, 2); 

                dividendCalculations.WithholdingTaxRate = dividend.ISIN.Substring(0, 2).Equals("US") ? 15 : dividend.WithholdingTaxRate;

                dividendCalculations.WithholdingTaxPaid = Math.Round(dividendCalculations.DividendReceivedExchanged * dividendCalculations.WithholdingTaxRate / 100 , 2);
                
                decimal totalToBePaid = Math.Round(dividendCalculations.DividendReceivedExchanged * 0.19m, 2) - dividendCalculations.WithholdingTaxPaid;

                dividendCalculations.WithholdingTaxRemain = totalToBePaid > 0 ? totalToBePaid : 0;

                

                sum += dividendCalculations.DividendReceivedExchanged;
                taxPaid += dividendCalculations.WithholdingTaxPaid;
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
    }
}