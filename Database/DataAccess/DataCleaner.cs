using Database.DataAccess.Interfaces;
using System.Threading.Tasks;

namespace Database.DataAccess
{
    public class DataCleaner : IDataCleaner
    {
        async Task IDataCleaner.CleanData()
        {
            using (var context = new ApplicationDbContext())
            {
                context.RemoveRange(context.ClosedPositions);
                context.RemoveRange(context.TransactionReports);
                context.RemoveRange(context.CfdCalculations);
                context.RemoveRange(context.CryptoCalculations);
                context.RemoveRange(context.StockCalculations);
                //context.RemoveRange(context.ExchangeRates);
                await context.SaveChangesAsync();
            }
        }
    }
}