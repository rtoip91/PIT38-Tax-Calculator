﻿using Database.DataAccess.Interfaces;
using System.Collections.Generic;
using Database.Entities.InMemory;
using Database.Repository;

namespace Database.DataAccess
{
    public sealed class StockEntityDataAccess : IStockEntityDataAccess
    {
        private readonly IDataRepository _dataRepository;

        public StockEntityDataAccess(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        public void AddEntities(IList<StockEntity> stockEntities)
        {
            foreach (var stockEntity in stockEntities)
            {
                _dataRepository.StockCalculations.Add(stockEntity);
            }
        }

        public IList<StockEntity> GetEntities()
        {
            return _dataRepository.StockCalculations;
        }
    }
}