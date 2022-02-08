using System.Collections.Generic;
using System.Threading.Tasks;
using Database.DataAccess.Interfaces;
using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database.DataAccess
{
    public class CryptoEntityDataAccess : ICryptoEntityDataAccess
    {
        public async Task<int> AddEntities(IList<CryptoEntity> cryptoEntities)
        {
            await using var context = new ApplicationDbContext();
            await context.AddRangeAsync(cryptoEntities);
            return await context.SaveChangesAsync();
        }

        public async Task<IList<CryptoEntity>> GetCryptoEntities()
        {
            await using var context = new ApplicationDbContext();
            return await context.CryptoCalculations.ToListAsync();
        }
    }
}