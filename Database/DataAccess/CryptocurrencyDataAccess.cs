using System.Linq;
using System.Threading.Tasks;
using Database.DataAccess.Interfaces;
using Database.Entities.Database;

namespace Database.DataAccess;

internal sealed class CryptocurrencyDataAccess : ICryptocurrencyDataAccess
{
    public async Task<bool> SaveCryptocurrency(CryptocurrencyEntity cryptocurrencyEntity)
    {
        await using var context = new ApplicationDbContext();
        await context.CryptocurrencyEntities.AddAsync(cryptocurrencyEntity);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<string> GetName(string code)
    {
        await using var context = new ApplicationDbContext();
        return context.CryptocurrencyEntities.FirstOrDefault(e => e.Code == code)?.Name;
    }
}