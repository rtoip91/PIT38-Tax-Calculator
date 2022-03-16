using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.DataAccess.Interfaces;
using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database.DataAccess
{
    public  class DividendsDataAccess : IDividendsDataAccess
    {
        public async Task<int> AddDividends(IList<DividendEntity> dividends)
        {
            await using var context = new ApplicationDbContext();
            await context.AddRangeAsync(dividends);
            return await context.SaveChangesAsync();
        }

        public async Task<IList<DividendEntity>> GetDividends()
        {
            await using var context = new ApplicationDbContext();
            return await context.Dividends.ToListAsync();
        }
    }
}
