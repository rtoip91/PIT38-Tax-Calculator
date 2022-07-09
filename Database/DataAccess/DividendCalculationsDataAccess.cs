using System.Collections.Generic;
using Database.DataAccess.Interfaces;
using Database.Entities;
using Database.Entities.InMemory;
using Database.Repository;

namespace Database.DataAccess
{
    public class DividendCalculationsDataAccess : IDividendCalculationsDataAccess
    {
        private readonly IDataRepository _dataRepository;

        public DividendCalculationsDataAccess(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        public void AddEntities(IList<DividendCalculationsEntity> dividendCalculationsEntities)
        {
            foreach (var dividendCalculationsEntity in dividendCalculationsEntities)
            {
                _dataRepository.DividendsCalculations.Add(dividendCalculationsEntity);
            }
        }

        public IList<DividendCalculationsEntity> GetEntities()
        {
            return _dataRepository.DividendsCalculations;
        }
    }
}