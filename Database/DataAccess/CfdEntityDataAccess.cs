using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.DataAccess.Interfaces;
using Database.Entities;

namespace Database.DataAccess
{
    public class CfdEntityDataAccess : ICfdEntityDataAccess
    {
        public async Task<int> AddEntities(IList<CfdEntity> cfdEntities)
        {
            await using var context = new ApplicationDbContext();
            await context.AddRangeAsync(cfdEntities);
            return await context.SaveChangesAsync();
        }
    }
}