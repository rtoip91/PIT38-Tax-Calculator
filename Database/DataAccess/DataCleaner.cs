using Database.DataAccess.Interfaces;
using System.Threading.Tasks;

namespace Database.DataAccess;

public class DataCleaner : IDataCleaner
{
    async Task IDataCleaner.CleanData()
    {
        await using var context = new ApplicationDbContext();
        //context.RemoveRange(context.ExchangeRates);
        await context.SaveChangesAsync();
    }
}