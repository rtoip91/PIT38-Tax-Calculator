using System.Collections.Generic;
using System.Threading.Tasks;
using Database.DataAccess.Interfaces;
using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database.DataAccess
{
    public class PurchasedCryptoEntityDataAccess : IPurchasedCryptoEntityDataAccess
    {
        public async Task<int> AddEntities(IList<PurchasedCryptoEntity> purchasedCryptoEntities)
        {
            await using var context = new ApplicationDbContext();
            await context.AddRangeAsync(purchasedCryptoEntities);
            return await context.SaveChangesAsync();
        }

        public async Task<IList<PurchasedCryptoEntity>> GetPurchasedCryptoEntities()
        {
            await using var context = new ApplicationDbContext();
            return await context.PurchasedCryptoCalculations.ToListAsync();
        }
    }
}
