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
        private readonly ITransactionReportsDataAccess _transactionReportsDataAccess;
        private readonly ISoldCryptoEntityDataAccess _soldCryptoEntityDataAccess;
        private readonly IPurchasedCryptoEntityDataAccess _purchasedCryptoEntityDataAccess;

        public CryptoCalculator(IExchangeRates exchangeRates,
            IClosedPositionsDataAccess closedPositionsDataAccess,
            ISoldCryptoEntityDataAccess soldCryptoEntityDataAccess,
            IPurchasedCryptoEntityDataAccess purchasedCryptoEntityDataAccess,
            ITransactionReportsDataAccess transactionReportsDataAccess)
        {
            _exchangeRates = exchangeRates;
            _closedPositionsDataAccess = closedPositionsDataAccess;
            _soldCryptoEntityDataAccess = soldCryptoEntityDataAccess;
            _purchasedCryptoEntityDataAccess = purchasedCryptoEntityDataAccess;
            _transactionReportsDataAccess = transactionReportsDataAccess;
        }

        public async Task<T> Calculate<T>() where T : CryptoDto
        {
            IList<PurchasedCryptoEntity> purchasedCryptoEntities = new List<PurchasedCryptoEntity>();
            IList<SoldCryptoEntity> soldCryptoEntities = new List<SoldCryptoEntity>();

            var cryptoClosedPositions = await _closedPositionsDataAccess.GetCryptoPositions();

                foreach (var cryptoClosedPosition in cryptoClosedPositions)
                {
                    PurchasedCryptoEntity purchasedCryptoEntity = await CreatePurchasedCryptoEntity(cryptoClosedPosition);
                    SoldCryptoEntity soldCryptoEntity = await CreateSoldCryptoEntity(cryptoClosedPosition);

                    purchasedCryptoEntities.Add(purchasedCryptoEntity);
                    soldCryptoEntities.Add(soldCryptoEntity);

                    await _closedPositionsDataAccess.RemovePosition(cryptoClosedPosition);
                }
            

            try
            {
                await _purchasedCryptoEntityDataAccess.AddEntities(purchasedCryptoEntities);
                await _soldCryptoEntityDataAccess.AddEntities(soldCryptoEntities);
                decimal totalLoss = purchasedCryptoEntities.Where(c=>c.PurchaseDate.Year == c.PurchaseDate.Year).Sum(c => c.TotalExchangedValue);
                decimal totalGain = soldCryptoEntities.Sum(c => c.TotalExchangedValue);
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

        private async Task<SoldCryptoEntity> CreateSoldCryptoEntity(ClosedPositionEntity cryptoClosedPosition) 
        {
            SoldCryptoEntity soldCryptoEntity = new SoldCryptoEntity
            {
                Name = cryptoClosedPosition.Operation,
                SellDate = cryptoClosedPosition.ClosingDate,
                CurrencySymbol = "USD",
                PositionId = cryptoClosedPosition.PositionId ?? 0,
                Units = cryptoClosedPosition.Units ?? 0,
                ValuePerUnit = cryptoClosedPosition.ClosingRate ?? 0
            };

            ExchangeRateEntity soldExchangeRate =
                await _exchangeRates.GetRateForPreviousDay(soldCryptoEntity.CurrencySymbol, soldCryptoEntity.SellDate);
            soldCryptoEntity.ExchangeRate = soldExchangeRate.Rate;
            soldCryptoEntity.TotalValue = Math.Round(soldCryptoEntity.ValuePerUnit * soldCryptoEntity.Units, 2);
            soldCryptoEntity.TotalExchangedValue = Math.Round(soldCryptoEntity.TotalValue * soldCryptoEntity.ExchangeRate, 2);
            return soldCryptoEntity;
        }

        private async Task<PurchasedCryptoEntity> CreatePurchasedCryptoEntity(ClosedPositionEntity cryptoClosedPosition)
        {
            PurchasedCryptoEntity purchasedCryptoEntity = new PurchasedCryptoEntity
            {
                Name = cryptoClosedPosition.Operation,
                PurchaseDate = cryptoClosedPosition.OpeningDate,
                CurrencySymbol = "USD",
                PositionId = cryptoClosedPosition.PositionId ?? 0,
                Units = cryptoClosedPosition.Units ?? 0,
                ValuePerUnit = cryptoClosedPosition.OpeningRate ?? 0
            };

            ExchangeRateEntity purchasedExchangeRate =
                await _exchangeRates.GetRateForPreviousDay(purchasedCryptoEntity.CurrencySymbol,
                    purchasedCryptoEntity.PurchaseDate);
            purchasedCryptoEntity.ExchangeRate = purchasedExchangeRate.Rate;
            purchasedCryptoEntity.TotalValue = Math.Round(purchasedCryptoEntity.ValuePerUnit * purchasedCryptoEntity.Units, 2);
            purchasedCryptoEntity.TotalExchangedValue =
                Math.Round(purchasedCryptoEntity.TotalValue * purchasedCryptoEntity.ExchangeRate, 2);
            return purchasedCryptoEntity;
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