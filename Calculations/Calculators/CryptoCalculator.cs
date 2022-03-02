using Calculations.Dto;
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
            var cryptoClosedPositions = await _closedPositionsDataAccess.GetCryptoPositions();

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

                    Task<ExchangeRateEntity> closingRateTask = _exchangeRates.GetRateForPreviousDay(cryptoEntity.CurrencySymbol, cryptoEntity.SellDate);

                    Task<ExchangeRateEntity> openingRateTask = _exchangeRates.GetRateForPreviousDay(cryptoEntity.CurrencySymbol, cryptoEntity.PurchaseDate);


                    cryptoEntity.ClosingValue = cryptoEntity.OpeningValue + cryptoEntity.Profit;


                    await Task.WhenAll(closingRateTask, openingRateTask);
                    cryptoEntity.ClosingExchangeRate = closingRateTask.Result.Rate;
                    cryptoEntity.OpeningExchangeRate = openingRateTask.Result.Rate;               

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
                decimal totalLoss = cryptoEntities.Where(c=>c.PurchaseDate.Year == c.SellDate.Year).Sum(c => c.LossExchangedValue);
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