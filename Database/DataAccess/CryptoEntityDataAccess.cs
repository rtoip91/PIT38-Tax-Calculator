using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.DataAccess.Interfaces;
using Database.Entities;

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
    }
}
