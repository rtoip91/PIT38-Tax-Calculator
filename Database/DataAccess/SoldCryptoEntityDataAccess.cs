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
    public class SoldCryptoEntityDataAccess : ISoldCryptoEntityDataAccess
    {
        public async Task<int> AddEntities(IList<SoldCryptoEntity> soldCryptoEntities)
        {
            await using var context = new ApplicationDbContext();
            await context.AddRangeAsync(soldCryptoEntities);
            return await context.SaveChangesAsync();
        }

        public async Task<IList<SoldCryptoEntity>> GetSoldCryptoEntities()
        {
            await using var context = new ApplicationDbContext();
            return await context.SoldCryptoCalculations.ToListAsync();
        }
    }
}
