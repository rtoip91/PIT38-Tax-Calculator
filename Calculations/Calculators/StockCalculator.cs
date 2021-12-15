using Calculations.Dto;
using Calculations.Interfaces;
using Database;
using Database.DataAccess.Interfaces;
using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Calculations.Calculators
{
    class StockCalculator : ICalculator<StockCalculatorDto>
    {
        private readonly IExchangeRates _exchangeRates;
        private readonly IClosedPositionsDataAccess _closedPositionDataAccess;
        private readonly IStockEntityDataAccess _stockEntityDataAccess;

        public StockCalculator(IExchangeRates exchangeRates,
            IClosedPositionsDataAccess closedPositionsDataAccess,
            IStockEntityDataAccess stockEntityDataAccess)
        {
            _exchangeRates = exchangeRates;
            _closedPositionDataAccess = closedPositionsDataAccess;
            _stockEntityDataAccess = stockEntityDataAccess;
        }

        public async Task<T> Calculate<T>() where T : StockCalculatorDto
        {
            IList<StockEntity> stockEntities = new List<StockEntity>();            
            var stockClosedPositions = await _closedPositionDataAccess.GetStockPositions();

            foreach (var stockClosedPosition in stockClosedPositions)
            {
                StockEntity stockEntity = new StockEntity
                {
                    Name = stockClosedPosition.Operation,
                    PurchaseDate = stockClosedPosition.OpeningDate,
                    SellDate = stockClosedPosition.ClosingDate,
                    CurrencySymbol = "USD",
                    PositionId = stockClosedPosition.PositionId ?? 0
                };

                Task<ExchangeRateEntity> closingRateTask = _exchangeRates.GetRateForPreviousDay(stockEntity.CurrencySymbol, stockEntity.SellDate);
                Task<ExchangeRateEntity> openingRateTask = _exchangeRates.GetRateForPreviousDay(stockEntity.CurrencySymbol, stockEntity.PurchaseDate);

                stockEntity.OpeningValue = stockClosedPosition.OpeningRate * stockClosedPosition.Units ?? 0;
                stockEntity.ClosingValue = stockClosedPosition.ClosingRate * stockClosedPosition.Units ?? 0;
                stockEntity.Profit = stockEntity.ClosingValue - stockEntity.OpeningValue;

                await Task.WhenAll(closingRateTask, openingRateTask);
                stockEntity.ClosingExchangeRate = closingRateTask.Result.Rate;
                stockEntity.OpeningExchangeRate = openingRateTask.Result.Rate;

                stockEntity.LossExchangedValue =
                    Math.Round(stockEntity.OpeningValue * stockEntity.OpeningExchangeRate, 2);
                stockEntity.GainExchangedValue =
                    Math.Round(stockEntity.ClosingValue * stockEntity.ClosingExchangeRate, 2);

                stockEntities.Add(stockEntity);

                await _closedPositionDataAccess.RemovePosition(stockClosedPosition);
            }

            try
            {
                await _stockEntityDataAccess.AddEntities(stockEntities);
                decimal totalLoss = stockEntities.Sum(c => c.LossExchangedValue);
                decimal totalGain = stockEntities.Sum(c => c.GainExchangedValue);

                var stockCalculatorDto = new StockCalculatorDto
                {
                    Cost = totalLoss,
                    Revenue = totalGain,
                    Income = totalGain - totalLoss
                };

                return (T) stockCalculatorDto;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }        
    }
}