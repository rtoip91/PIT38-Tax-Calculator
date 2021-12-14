using Database.DataAccess.Interfaces;
using Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.DataAccess
{
    public class StockEntityDataAccess : IStockEntityDataAccess
    {
        public async Task<int> AddEntities(IList<StockEntity> stockEntities)
        {
            await using var context = new ApplicationDbContext();
            await context.AddRangeAsync(stockEntities);
            return await context.SaveChangesAsync();
        }
    }
}
