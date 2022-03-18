using Database.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Database.DataAccess.Interfaces
{
    public interface IStockEntityDataAccess
    {
        void AddEntities(IList<StockEntity> stockEntities);
        IList<StockEntity> GetEntities();
    }
}