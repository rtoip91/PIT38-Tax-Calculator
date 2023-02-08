using Calculations.Dto;
using Calculations.Extensions;
using Calculations.Interfaces;
using Database.DataAccess.Interfaces;
using Database.Entities.Database;
using Database.Entities.InMemory;

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

        public async Task<T?> Calculate<T>() where T : CryptoDto
        {
            IList<PurchasedCryptoEntity> purchasedCryptoEntities = new List<PurchasedCryptoEntity>();
            IList<SoldCryptoEntity> soldCryptoEntities = new List<SoldCryptoEntity>();

            var cryptoClosedPositions = _closedPositionsDataAccess.GetCryptoPositions();

            foreach (var cryptoClosedPosition in cryptoClosedPositions)
            {
                if (cryptoClosedPosition.OpeningDate.Year == cryptoClosedPosition.ClosingDate.Year)
                {
                    PurchasedCryptoEntity purchasedCryptoEntity =
                        await CreatePurchasedCryptoEntity(cryptoClosedPosition);
                    purchasedCryptoEntities.Add(purchasedCryptoEntity);
                }

                SoldCryptoEntity soldCryptoEntity = await CreateSoldCryptoEntity(cryptoClosedPosition);


                soldCryptoEntities.Add(soldCryptoEntity);

                _closedPositionsDataAccess.RemovePosition(cryptoClosedPosition);
            }

            await AddUnsoldCryptos(purchasedCryptoEntities);

            try
            {
                _purchasedCryptoEntityDataAccess.AddEntities(purchasedCryptoEntities);
                _soldCryptoEntityDataAccess.AddEntities(soldCryptoEntities);
                decimal totalLoss = purchasedCryptoEntities.Sum(c => c.TotalExchangedValue);
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

        private string SoldOperationName(string name)
        {
            int index = name.IndexOf(" ", StringComparison.Ordinal);
            return $"Sprzedaj{name.Substring(index)}";
        }

        private string PurchasedOperationName(string name)
        {
            int index = name.IndexOf(" ", StringComparison.Ordinal);
            return $"Kup{name.Substring(index)}";
        }

        private async Task<SoldCryptoEntity> CreateSoldCryptoEntity(ClosedPositionEntity cryptoClosedPosition)
        {
            SoldCryptoEntity soldCryptoEntity = new SoldCryptoEntity
            {
                Name = SoldOperationName(cryptoClosedPosition.Operation),
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
            soldCryptoEntity.TotalExchangedValue =
                (soldCryptoEntity.TotalValue * soldCryptoEntity.ExchangeRate).RoundDecimal();
            return soldCryptoEntity;
        }

        private async Task<PurchasedCryptoEntity> CreatePurchasedCryptoEntity(ClosedPositionEntity cryptoClosedPosition)
        {
            PurchasedCryptoEntity purchasedCryptoEntity = new PurchasedCryptoEntity
            {
                Name = PurchasedOperationName(cryptoClosedPosition.Operation),
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
            purchasedCryptoEntity.TotalValue =
                (purchasedCryptoEntity.ValuePerUnit * purchasedCryptoEntity.Units).RoundDecimal();
            purchasedCryptoEntity.TotalExchangedValue =
                (purchasedCryptoEntity.TotalValue * purchasedCryptoEntity.ExchangeRate).RoundDecimal();
            return purchasedCryptoEntity;
        }

        private async Task AddUnsoldCryptos(IList<PurchasedCryptoEntity> purchasedCryptoEntities)
        {
            var transReports = _transactionReportsDataAccess.GetUnsoldCryptoTransactions();
            foreach (var transaction in transReports)
            {
                PurchasedCryptoEntity purchasedCryptoEntity = new PurchasedCryptoEntity
                {
                    CurrencySymbol = "USD",
                    PurchaseDate = transaction.Date,
                    PositionId = transaction.PositionId ?? 0,
                    TotalValue = transaction.Amount
                };

                purchasedCryptoEntity.Name = GetCryptoCurrencyName(transaction);
                ExchangeRateEntity purchasedExchangeRate =
                    await _exchangeRates.GetRateForPreviousDay(purchasedCryptoEntity.CurrencySymbol,
                        purchasedCryptoEntity.PurchaseDate);
                purchasedCryptoEntity.ExchangeRate = purchasedExchangeRate.Rate;
                purchasedCryptoEntity.ExchangeRateDate = purchasedExchangeRate.Date;
                purchasedCryptoEntity.TotalExchangedValue =
                    (purchasedCryptoEntity.TotalValue * purchasedCryptoEntity.ExchangeRate).RoundDecimal();
                purchasedCryptoEntities.Add(purchasedCryptoEntity);
            }
        }

        private string GetCryptoCurrencyName(TransactionReportEntity transactionReport)
        {
            string? name = transactionReport.Details.Split('/').FirstOrDefault();

            var result =
                Dictionaries.Dictionaries.CryptoCurrenciesDictionary.TryGetValue(name, out string cryptoName);
            return result ? $"Kup {cryptoName}" : $"Kup {transactionReport.Details}";
        }
    }
}