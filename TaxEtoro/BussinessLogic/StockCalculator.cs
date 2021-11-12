using Database;
using Database.Entities;
using EtoroExcelReader.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxEtoro.Interfaces;

namespace TaxEtoro.BussinessLogic
{
    class StockCalculator : ICalculator
    {

        private readonly IExchangeRatesGetter _exchangeRatesGetter;

        public StockCalculator(IExchangeRatesGetter exchangeRatesGetter)
        {
            _exchangeRatesGetter = exchangeRatesGetter;
        }

        public async Task Calculate()
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

                    Console.WriteLine("Akcje:");
                    Console.WriteLine($"Koszt zakupu = {totalLoss}");
                    Console.WriteLine($"Przychód = {totalGain}");
                    Console.WriteLine($"Dochód = {totalGain - totalLoss}");
                    Console.WriteLine();                  
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

            }

        }       
    }
}
