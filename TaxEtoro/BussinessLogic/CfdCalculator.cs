using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using TaxEtoro.BussinessLogic.Dto;
using TaxEtoro.Interfaces;

namespace TaxEtoro.BussinessLogic
{
    internal class CfdCalculator : ICalculator<CfdCalculatorDto>
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
                        PositionId = cfdClosedPosition.PositionId ?? 0
                    };


                    ExchangeRateEntity exchangeRateEntity = await _exchangeRatesGetter.GetRateForPreviousDay(cfdEntity.CurrencySymbol, cfdEntity.SellDate);

                    cfdEntity.ExchangeRate = exchangeRateEntity.Rate;

                    var openingValue = cfdClosedPosition.OpeningRate * cfdClosedPosition.Units ?? 0;
                    var closingValue = cfdClosedPosition.ClosingRate * cfdClosedPosition.Units ?? 0;                  

                    if(cfdEntity.Name.ToLower().Contains("buy"))
                    {
                        cfdEntity.GainValue = Math.Round(closingValue - openingValue, 2);
                    }

                    if(cfdEntity.Name.ToLower().Contains("sell"))
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
                    Console.WriteLine($"Zysk = {totalGain} PLN");
                    Console.WriteLine($"Strata = {totalLoss} PLN");
                    Console.WriteLine($"Dochód = {totalGain + totalLoss} PLN");
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
