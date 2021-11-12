using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using TaxEtoro.Interfaces;

namespace TaxEtoro.BussinessLogic
{
    internal class CfdCalculator : ICalculator
    {

        private readonly IExchangeRatesGetter _exchangeRatesGetter;

        public CfdCalculator (IExchangeRatesGetter exchangeRatesGetter)
        {
            _exchangeRatesGetter = exchangeRatesGetter;
        }

        public async Task Calculate()
        {
            using (var context = new ApplicationDbContext())
            {
                var cfdClosedPositions = context.ClosedPositions.Where(c => c.IsReal.Contains("CFD")).Include(c => c.TransactionReports);
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
                        GainValue = cfdClosedPosition.Profit ?? 0,
                        PositionId = cfdClosedPosition.PositionId ?? 0
                    };


                    ExchangeRateEntity exchangeRateEntity = await _exchangeRatesGetter.GetRateForPreviousDay(cfdEntity.CurrencySymbol, cfdEntity.SellDate);

                    cfdEntity.ExchangeRate = exchangeRateEntity.Rate;

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

                    if (cfdClosedPosition.TransactionReports != null)
                    {
                        context.RemoveRange(cfdClosedPosition.TransactionReports);
                    }

                    context.Remove(cfdClosedPosition);
                }

                await context.AddRangeAsync(cfdEntities);

                try
                {
                    await context.SaveChangesAsync();

                    decimal totalLoss = cfdEntities.Sum(c => c.LossExchangedValue);
                    decimal totalGain = cfdEntities.Sum(c => c.GainExchangedValue);

                    Console.WriteLine("CFD:");
                    Console.WriteLine($"Zysk = {totalGain}");
                    Console.WriteLine($"Strata = {totalLoss}");
                    Console.WriteLine($"Dochód = {totalGain + totalLoss}");
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
