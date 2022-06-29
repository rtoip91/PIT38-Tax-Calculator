using Database.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Database.Entities.InMemory;

namespace Database.DataAccess.Interfaces
{
    public interface IStockEntityDataAccess
    {
        void AddEntities(IList<StockEntity> stockEntities);
        IList<StockEntity> GetEntities();
    }
}