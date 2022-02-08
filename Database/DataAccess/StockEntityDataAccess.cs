using Database.DataAccess.Interfaces;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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

        public async Task<IList<StockEntity>> GetEntities()
        {
            await using var context = new ApplicationDbContext();
            return await context.StockCalculations.ToListAsync();
        }
    }
}