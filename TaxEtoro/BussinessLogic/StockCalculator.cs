using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxEtoro.BussinessLogic.Dto;
using TaxEtoro.Interfaces;

namespace TaxEtoro.BussinessLogic
{
    class StockCalculator : ICalculator<StockCalculatorDto>
    {

        private readonly IExchangeRatesGetter _exchangeRatesGetter;

        public StockCalculator(IExchangeRatesGetter exchangeRatesGetter)
        {
            _exchangeRatesGetter = exchangeRatesGetter;
        }

        public async Task<T> Calculate<T>() where T : StockCalculatorDto
        {
            using (var context = new ApplicationDbContext())
            {
                IList<StockEntity> stockEntities = new List<StockEntity>();
                var stockClosedPositions = context.ClosedPositions.Include(c => c.TransactionReports);

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
                    
                    stockEntity.OpeningValue = stockClosedPosition.OpeningRate * stockClosedPosition.Units ?? 0;
                    stockEntity.ClosingValue = stockClosedPosition.ClosingRate * stockClosedPosition.Units ?? 0;
                    stockEntity.Profit = stockEntity.ClosingValue - stockEntity.OpeningValue;                    

                    ExchangeRateEntity exchangeRateEntity = await _exchangeRatesGetter.GetRateForPreviousDay(stockEntity.CurrencySymbol, stockEntity.SellDate);
                    stockEntity.ClosingExchangeRate = exchangeRateEntity.Rate;

                    exchangeRateEntity = await _exchangeRatesGetter.GetRateForPreviousDay(stockEntity.CurrencySymbol, stockEntity.PurchaseDate);
                    stockEntity.OpeningExchangeRate = exchangeRateEntity.Rate;

                    stockEntity.LossExchangedValue = Math.Round(stockEntity.OpeningValue * stockEntity.OpeningExchangeRate, 2);
                    stockEntity.GainExchangedValue = Math.Round(stockEntity.ClosingValue * stockEntity.ClosingExchangeRate, 2);


                    stockEntities.Add(stockEntity);

                    if (stockClosedPosition.TransactionReports != null)
                    {
                        context.RemoveRange(stockClosedPosition.TransactionReports);
                    }

                    context.Remove(stockClosedPosition);
                }

                await context.AddRangeAsync(stockEntities);

                try
                {
                    await context.SaveChangesAsync();
                    decimal totalLoss = stockEntities.Sum(c => c.LossExchangedValue);
                    decimal totalGain = stockEntities.Sum(c => c.GainExchangedValue);

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
}
