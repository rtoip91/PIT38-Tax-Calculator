using Database.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Database.DataAccess.Interfaces
{
    public interface IStockEntityDataAccess
    {
        Task<int> AddEntities(IList<StockEntity> stockEntities);
    }
}