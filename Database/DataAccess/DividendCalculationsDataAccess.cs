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
    public  class DividendCalculationsDataAccess : IDividendCalculationsDataAccess
    {
        public async Task<int> AddEntities(IList<DividendCalculationsEntity> dividendCalculationsEntities)
        {

            await using var context = new ApplicationDbContext();
            await context.AddRangeAsync(dividendCalculationsEntities);
            return await context.SaveChangesAsync();
        }

        public async Task<IList<DividendCalculationsEntity>> GetEntities()
        {
            await using var context = new ApplicationDbContext();
            return await context.DividendsCalculations.ToListAsync();
        }
    }
}
