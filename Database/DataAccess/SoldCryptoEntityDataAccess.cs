using System.Collections.Generic;
using Database.DataAccess.Interfaces;
using Database.Entities;
using Database.Repository;

namespace Database.DataAccess
{
    public class SoldCryptoEntityDataAccess : ISoldCryptoEntityDataAccess
    {
        private readonly IDataRepository _dataRepository;

        public SoldCryptoEntityDataAccess(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        public void AddEntities(IList<SoldCryptoEntity> soldCryptoEntities)
        {
            foreach (var soldCryptoEntity in soldCryptoEntities)
            {
                _dataRepository.SoldCryptoCalculations.Add(soldCryptoEntity);
            }
        }

        public IList<SoldCryptoEntity> GetSoldCryptoEntities()
        {
            return _dataRepository.SoldCryptoCalculations;
        }
    }
}