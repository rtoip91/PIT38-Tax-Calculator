using System.Globalization;
using Calculations.Dto;
using Calculations.Extensions;
using Calculations.Interfaces;
using Database.DataAccess.Interfaces;
using Database.Entities;
using Database.Enums;

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
                    PositionId = cfdClosedPosition.PositionId ?? 0,
                    Leverage = cfdClosedPosition.Leverage,
                    TransactionType = cfdClosedPosition.TransactionType
                };

                RegionInfo regionInfo = new RegionInfo(cfdClosedPosition.ISIN);
                cfdEntity.Country = regionInfo.EnglishName;

                ExchangeRateEntity exchangeRate = await _exchangeRates.GetRateForPreviousDay(cfdEntity.CurrencySymbol, cfdEntity.SellDate);

                var openingValue = (cfdEntity.OpeningRate * cfdEntity.Units).RoundDecimal(4);
                var closingValue = (cfdEntity.ClosingRate * cfdEntity.Units).RoundDecimal(4);

                if (cfdEntity.TransactionType == TransactionType.Long)
                {
                    cfdEntity.GainValue = closingValue - openingValue;
                }

                if (cfdEntity.TransactionType == TransactionType.Short)
                {
                    cfdEntity.GainValue = openingValue - closingValue;
                }

                cfdEntity.ExchangeRate = exchangeRate.Rate;
                cfdEntity.ExchangeRateDate = exchangeRate.Date;

                decimal exchangedValue = (cfdEntity.GainValue * cfdEntity.ExchangeRate).RoundDecimal();
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