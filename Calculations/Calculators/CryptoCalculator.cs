using Calculations.Dto;
using Calculations.Extensions;
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

                await AddUnsoldCryptos(purchasedCryptoEntities);            

            try
            {
                await _purchasedCryptoEntityDataAccess.AddEntities(purchasedCryptoEntities);
                await _soldCryptoEntityDataAccess.AddEntities(soldCryptoEntities);
                decimal totalLoss = purchasedCryptoEntities.Where(c=>c.PurchaseDate.Year == c.PurchaseDate.Year).Sum(c => c.TotalExchangedValue);
                decimal totalGain = soldCryptoEntities.Sum(c => c.TotalExchangedValue);

                var cryptoDto = new CryptoDto
                {
                    Cost = totalLoss,
                    Revenue = totalGain,
                    Income = totalGain - totalLoss
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
                Name = cryptoClosedPosition.Operation.Replace("Kupno", "Sprzedaż"),
                SellDate = cryptoClosedPosition.ClosingDate,
                CurrencySymbol = "USD",
                PositionId = cryptoClosedPosition.PositionId ?? 0,
                Units = cryptoClosedPosition.Units ?? 0,
                ValuePerUnit = cryptoClosedPosition.ClosingRate ?? 0
            };

            ExchangeRateEntity soldExchangeRate =
                await _exchangeRates.GetRateForPreviousDay(soldCryptoEntity.CurrencySymbol, soldCryptoEntity.SellDate);
            soldCryptoEntity.ExchangeRate = soldExchangeRate.Rate;
            soldCryptoEntity.ExchangeRateDate = soldExchangeRate.Date;
            soldCryptoEntity.TotalValue = (soldCryptoEntity.ValuePerUnit * soldCryptoEntity.Units).RoundDecimal();
            soldCryptoEntity.TotalExchangedValue = (soldCryptoEntity.TotalValue * soldCryptoEntity.ExchangeRate).RoundDecimal();
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
            purchasedCryptoEntity.ExchangeRateDate = purchasedExchangeRate.Date;
            purchasedCryptoEntity.TotalValue = (purchasedCryptoEntity.ValuePerUnit * purchasedCryptoEntity.Units).RoundDecimal();
            purchasedCryptoEntity.TotalExchangedValue =(purchasedCryptoEntity.TotalValue * purchasedCryptoEntity.ExchangeRate).RoundDecimal();
            return purchasedCryptoEntity;
        }

        private async Task AddUnsoldCryptos(IList<PurchasedCryptoEntity> purchasedCryptoEntities)
        {
            IList<string> cryptoList = Dictionaries.Dictionaries.CryptoCurrenciesDictionary.Keys.ToList();
           
            foreach (var crypto in cryptoList)
            {
                var transReports = await _transactionReportsDataAccess.GetUnsoldCryptoTransactions(crypto);
                foreach (var transaction in transReports)
                {
                    PurchasedCryptoEntity purchasedCryptoEntity = new PurchasedCryptoEntity
                    {
                        CurrencySymbol = "USD",
                        PurchaseDate = transaction.Date,
                        PositionId = transaction.PositionId ?? 0,
                        TotalValue = transaction.Amount
                    };

                    purchasedCryptoEntity.Name = GetCryptoCurrencyName(crypto);
                    ExchangeRateEntity purchasedExchangeRate = await _exchangeRates.GetRateForPreviousDay(purchasedCryptoEntity.CurrencySymbol, purchasedCryptoEntity.PurchaseDate);
                    purchasedCryptoEntity.ExchangeRate = purchasedExchangeRate.Rate;
                    purchasedCryptoEntity.ExchangeRateDate = purchasedExchangeRate.Date;
                    purchasedCryptoEntity.TotalExchangedValue = (purchasedCryptoEntity.TotalValue * purchasedCryptoEntity.ExchangeRate).RoundDecimal();
                    purchasedCryptoEntities.Add(purchasedCryptoEntity);
                }
            }
        }

        private string GetCryptoCurrencyName(string crypto)
        {
            var result = Dictionaries.Dictionaries.CryptoCurrenciesDictionary.TryGetValue(crypto, out string cryptoName);
            return result ? $"Kupno {cryptoName}" : $"Kupno {crypto}";
        }
    }
}