using System.Collections.Generic;
using Database.DataAccess.Interfaces;
using Database.Entities;
using Database.Entities.InMemory;
using Database.Repository;

namespace Database.DataAccess
{
    public class PurchasedCryptoEntityDataAccess : IPurchasedCryptoEntityDataAccess
    {
        private readonly IDataRepository _dataRepository;

        public PurchasedCryptoEntityDataAccess(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        public void AddEntities(IList<PurchasedCryptoEntity> purchasedCryptoEntities)
        {
            foreach (var purchasedCryptoEntity in purchasedCryptoEntities)
            {
                _dataRepository.PurchasedCryptoCalculations.Add(purchasedCryptoEntity);
            }
        }

        public IList<PurchasedCryptoEntity> GetPurchasedCryptoEntities()
        {
            return _dataRepository.PurchasedCryptoCalculations;
        }
    }
}