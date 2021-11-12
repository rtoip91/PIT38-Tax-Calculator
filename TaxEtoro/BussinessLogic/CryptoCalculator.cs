using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Database.Entities;
using EtoroExcelReader.Dto;
using Microsoft.EntityFrameworkCore;
using TaxEtoro.Interfaces;

namespace TaxEtoro.BussinessLogic
{
    internal class CryptoCalculator : ICalculator
    {
        private readonly IExchangeRatesGetter _exchangeRatesGetter;

        public CryptoCalculator(IExchangeRatesGetter exchangeRatesGetter)
        {
            _exchangeRatesGetter = exchangeRatesGetter;
        }

        public async Task Calculate()
        {
            using (var context = new ApplicationDbContext())
            {
                IList<CryptoEntity> cryptoEntities = new List<CryptoEntity>();
                IList<string> cryptoList = Dictionaries.CryptoCurrenciesDictionary.Values.ToList();

                foreach (var crypto in cryptoList)
                {
                    var cryptoClosedPositions = context.ClosedPositions.Where(c => c.Operation.ToLower().Contains($" {crypto.ToLower()}"))
                        .Include(c => c.TransactionReports);

                    foreach (var cryptoClosedPosition in cryptoClosedPositions)
                    {
                        CryptoEntity cryptoEntity = new CryptoEntity
                        {
                            Name = cryptoClosedPosition.Operation,
                            PurchaseDate = cryptoClosedPosition.OpeningDate,
                            OpeningValue = cryptoClosedPosition.Amount ?? 0,                          
                            SellDate = cryptoClosedPosition.ClosingDate,                           
                            CurrencySymbol = "USD",
                            PositionId = cryptoClosedPosition.PositionId ?? 0,
                            Profit = cryptoClosedPosition.Profit ?? 0
                        };


                        cryptoEntity.ClosingValue = cryptoEntity.OpeningValue + cryptoEntity.Profit;

                        ExchangeRateEntity exchangeRateEntity = await _exchangeRatesGetter.GetRateForPreviousDay(cryptoEntity.CurrencySymbol, cryptoEntity.SellDate);
                        cryptoEntity.ClosingExchangeRate = exchangeRateEntity.Rate;
                        exchangeRateEntity = await _exchangeRatesGetter.GetRateForPreviousDay(cryptoEntity.CurrencySymbol, cryptoEntity.PurchaseDate);
                        cryptoEntity.OpeningExchangeRate = exchangeRateEntity.Rate;                   

                        cryptoEntity.LossExchangedValue = Math.Round(cryptoEntity.OpeningValue * cryptoEntity.OpeningExchangeRate, 2);                        
                        cryptoEntity.GainExchangedValue = Math.Round(cryptoEntity.ClosingValue * cryptoEntity.ClosingExchangeRate, 2);

                        cryptoEntities.Add(cryptoEntity);

                        if (cryptoClosedPosition.TransactionReports != null)
                        {
                            context.RemoveRange(cryptoClosedPosition.TransactionReports);
                        }

                        context.Remove(cryptoClosedPosition);
                    }

                }

                await context.AddRangeAsync(cryptoEntities);

                try
                {
                    await context.SaveChangesAsync();
                    decimal totalLoss = cryptoEntities.Sum(c => c.LossExchangedValue);
                    decimal totalGain = cryptoEntities.Sum(c => c.GainExchangedValue);

                   

                    Console.WriteLine("Kryptowaluty:");
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
