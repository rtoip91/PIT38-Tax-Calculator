using Database;
using System.Threading.Tasks;
using TaxEtoro.Interfaces;

namespace TaxEtoro.BussinessLogic
{
    public class DataCleaner : IDataCleaner
    {
        async Task IDataCleaner.CleanData()
        {
            using (var context = new ApplicationDbContext())
            {
                var closed = context.ClosedPositions;
                var transactionReports = context.TransactionReports;
                var cfdCalculations = context.CfdCalculations;
                var cryptoCalculations = context.CryptoCalculations;
                var stockCalculations = context.StockCalculations;

                context.RemoveRange(closed);
                context.RemoveRange(transactionReports);
                context.RemoveRange(cfdCalculations);
                context.RemoveRange(cryptoCalculations);
                context.RemoveRange(stockCalculations);

                await context.SaveChangesAsync();
                
            }
        }
    }
}
