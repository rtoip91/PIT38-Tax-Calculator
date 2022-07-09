using System.Collections.Generic;
using Database.DataAccess.Interfaces;
using Database.Entities;
using Database.Entities.InMemory;
using Database.Repository;

namespace Database.DataAccess
{
    public class CfdEntityDataAccess : ICfdEntityDataAccess
    {
        private readonly IDataRepository _dataRepository;

        public CfdEntityDataAccess(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        public void AddEntities(IList<CfdEntity> cfdEntities)
        {
            foreach (var cfdEntity in cfdEntities)
            {
                _dataRepository.CfdCalculations.Add(cfdEntity);
            }
        }

        public IList<CfdEntity> GetCfdEntities()
        {
            return _dataRepository.CfdCalculations;
        }
    }
}