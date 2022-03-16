using Calculations.Dto;
using Calculations.Interfaces;
using Database.DataAccess.Interfaces;
using Database.Entities;
using Calculations.Extensions;

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

                Task<ExchangeRateEntity> closingRateTask =
                    _exchangeRates.GetRateForPreviousDay(stockEntity.CurrencySymbol, stockEntity.SellDate);
                Task<ExchangeRateEntity> openingRateTask =
                    _exchangeRates.GetRateForPreviousDay(stockEntity.CurrencySymbol, stockEntity.PurchaseDate);

                stockEntity.OpeningUnitValue = (stockClosedPosition.OpeningRate ?? 0);
                stockEntity.ClosingUnitValue = (stockClosedPosition.ClosingRate ?? 0);
                stockEntity.Units = stockClosedPosition.Units ?? 0;

                stockEntity.OpeningValue = stockEntity.OpeningUnitValue * stockEntity.Units;
                stockEntity.ClosingValue = stockEntity.ClosingUnitValue * stockEntity.Units;
                stockEntity.Profit = stockEntity.ClosingValue - stockEntity.OpeningValue;

                await Task.WhenAll(closingRateTask, openingRateTask);
                stockEntity.ClosingExchangeRate = closingRateTask.Result.Rate;
                stockEntity.OpeningExchangeRate = openingRateTask.Result.Rate;

                stockEntity.OpeningExchangedValue = (stockEntity.OpeningValue * stockEntity.OpeningExchangeRate);
                stockEntity.ClosingExchangedValue = (stockEntity.ClosingValue * stockEntity.ClosingExchangeRate);

                stockEntity.ExchangedProfit = (stockEntity.ClosingExchangedValue - stockEntity.OpeningExchangedValue).RoundDecimal();

                stockEntities.Add(stockEntity);

                await _closedPositionDataAccess.RemovePosition(stockClosedPosition);
            }

            try
            {
                await _stockEntityDataAccess.AddEntities(stockEntities);
                var totalLoss = stockEntities.Sum(c => c.OpeningExchangedValue).RoundDecimal();
                var totalGain = stockEntities.Sum(c => c.ClosingExchangedValue).RoundDecimal();

                var stockCalculatorDto = new StockCalculatorDto
                {
                    Cost = totalLoss,
                    Revenue = totalGain,
                    Income = totalGain - totalLoss
                };

                return (T)stockCalculatorDto;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}