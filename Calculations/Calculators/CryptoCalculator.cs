﻿using Calculations.Dto;
using Calculations.Interfaces;
using Database.DataAccess.Interfaces;
using Database.Entities;

namespace Calculations.Calculators
{
    public class CryptoCalculator : ICalculator<CryptoDto>
    {
        private readonly IExchangeRates _exchangeRates;
        private readonly IClosedPositionsDataAccess _closedPositionsDataAccess;
        private readonly ICryptoEntityDataAccess _cryptoEntityDataAccess;
        private readonly ITransactionReportsDataAccess _transactionReportsDataAccess;

        public CryptoCalculator(IExchangeRates exchangeRates,
            IClosedPositionsDataAccess closedPositionsDataAccess,
            ICryptoEntityDataAccess cryptoEntityDataAccess,
            ITransactionReportsDataAccess transactionReportsDataAccess)
        {
            _exchangeRates = exchangeRates;
            _closedPositionsDataAccess = closedPositionsDataAccess;
            _cryptoEntityDataAccess = cryptoEntityDataAccess;
            _transactionReportsDataAccess = transactionReportsDataAccess;
        }

        public async Task<T> Calculate<T>() where T : CryptoDto
        {
            IList<CryptoEntity> cryptoEntities = new List<CryptoEntity>();
            IList<string> cryptoList = Dictionaries.Dictionaries.CryptoCurrenciesDictionary.Values.ToList();

           
                var cryptoClosedPositions = await _closedPositionsDataAccess.GetCryptoPositions(cryptoList);

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

                    ExchangeRateEntity exchangeRateEntity =
                        await _exchangeRates.GetRateForPreviousDay(cryptoEntity.CurrencySymbol, cryptoEntity.SellDate);
                    cryptoEntity.ClosingExchangeRate = exchangeRateEntity.Rate;
                    exchangeRateEntity =
                        await _exchangeRates.GetRateForPreviousDay(cryptoEntity.CurrencySymbol,
                            cryptoEntity.PurchaseDate);
                    cryptoEntity.OpeningExchangeRate = exchangeRateEntity.Rate;

                    cryptoEntity.LossExchangedValue =
                        Math.Round(cryptoEntity.OpeningValue * cryptoEntity.OpeningExchangeRate, 2);
                    cryptoEntity.GainExchangedValue =
                        Math.Round(cryptoEntity.ClosingValue * cryptoEntity.ClosingExchangeRate, 2);

                    cryptoEntities.Add(cryptoEntity);

                    await _closedPositionsDataAccess.RemovePosition(cryptoClosedPosition);
                }
            

            try
            {
                await _cryptoEntityDataAccess.AddEntities(cryptoEntities);
                decimal totalLoss = cryptoEntities.Sum(c => c.LossExchangedValue);
                decimal totalGain = cryptoEntities.Sum(c => c.GainExchangedValue);
                decimal unsoldCryptos = await UnsoldCryptos();

                var cryptoDto = new CryptoDto
                {
                    Cost = totalLoss,
                    Revenue = totalGain,
                    Income = totalGain - totalLoss,
                    UnsoldCryptos = unsoldCryptos,
                };

                return (T)cryptoDto;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private async Task<decimal> UnsoldCryptos()
        {
            IList<string> cryptoList = Dictionaries.Dictionaries.CryptoCurrenciesDictionary.Keys.ToList();
            decimal sum = 0;
            foreach (var crypto in cryptoList)
            {
                var transReports = await _transactionReportsDataAccess.GetUnsoldCryptoTransactions(crypto);
                foreach (var transaction in transReports)
                {
                    ExchangeRateEntity exchangeRateEntity =
                        await _exchangeRates.GetRateForPreviousDay("USD", transaction.Date);
                    decimal value = transaction.Amount * exchangeRateEntity.Rate;
                    sum += value;
                }
            }

            sum = Math.Round(sum, 2, MidpointRounding.AwayFromZero);
            return sum;
        }
    }
}