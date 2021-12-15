using Calculations.Dto;
using Calculations.Interfaces;
using Database;
using Database.DataAccess.Interfaces;
using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Calculations.Calculators
{
    public class CfdCalculator : ICalculator<CfdCalculatorDto>
    {
        private readonly IExchangeRates _exchangeRates;
        private readonly IClosedPositionsDataAccess _closedPositionsDataAccess;
        private readonly ICfdEntityDataAccess _cfdEntityDataAccess;

        public CfdCalculator(IExchangeRates exchangeRatesGetter,
            IClosedPositionsDataAccess closedPositionsDataAccess,
            ICfdEntityDataAccess cfdEntityDataAccess)
        {
            _exchangeRates = exchangeRatesGetter;
            _closedPositionsDataAccess = closedPositionsDataAccess;
            _cfdEntityDataAccess = cfdEntityDataAccess;
        }

        public async Task<T> Calculate<T>() where T : CfdCalculatorDto
        {
            var cfdClosedPositions = await _closedPositionsDataAccess.GetCfdPositions();
            IList<CfdEntity> cfdEntities = new List<CfdEntity>();

            foreach (var cfdClosedPosition in cfdClosedPositions)
            {
                CfdEntity cfdEntity = new CfdEntity
                {
                    Name = cfdClosedPosition.Operation,
                    PurchaseDate = cfdClosedPosition.OpeningDate,
                    OpeningRate = cfdClosedPosition.OpeningRate ?? 0,
                    ClosingRate = cfdClosedPosition.ClosingRate ?? 0,
                    SellDate = cfdClosedPosition.ClosingDate,
                    Units = cfdClosedPosition.Units ?? 0,
                    CurrencySymbol = "USD",
                    PositionId = cfdClosedPosition.PositionId ?? 0
                };


                ExchangeRateEntity exchangeRateEntity =
                    await _exchangeRates.GetRateForPreviousDay(cfdEntity.CurrencySymbol, cfdEntity.SellDate);

                cfdEntity.ExchangeRate = exchangeRateEntity.Rate;

                var openingValue = cfdClosedPosition.OpeningRate * cfdClosedPosition.Units ?? 0;
                var closingValue = cfdClosedPosition.ClosingRate * cfdClosedPosition.Units ?? 0;

                if (cfdEntity.Name.ToLower().Contains("buy"))
                {
                    cfdEntity.GainValue = Math.Round(closingValue - openingValue, 2);
                }

                if (cfdEntity.Name.ToLower().Contains("sell"))
                {
                    cfdEntity.GainValue = Math.Round(openingValue - closingValue, 2);
                }

                decimal exchangedValue = Math.Round(cfdEntity.GainValue * cfdEntity.ExchangeRate, 2);
                if (exchangedValue > 0)
                {
                    cfdEntity.GainExchangedValue = exchangedValue;
                }
                else
                {
                    cfdEntity.LossExchangedValue = exchangedValue;
                }

                cfdEntities.Add(cfdEntity);
                await _closedPositionsDataAccess.RemovePosition(cfdClosedPosition);
            }

            try
            {
                await _cfdEntityDataAccess.AddEntities(cfdEntities);

                decimal totalLoss = cfdEntities.Sum(c => c.LossExchangedValue);
                decimal totalGain = cfdEntities.Sum(c => c.GainExchangedValue);

                CfdCalculatorDto cfdCalculatorDto = new CfdCalculatorDto
                {
                    Loss = totalLoss,
                    Gain = totalGain,
                    Income = totalGain + totalLoss
                };

                return (T)cfdCalculatorDto;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}