using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.DataAccess.Interfaces;
using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database.DataAccess
{
    public class TransactionReportsDataAccess : ITransactionReportsDataAccess
    {
        public async Task<int> AddTransactionReports(IList<TransactionReportEntity> transactionReports)
        {
            await using var _dbContext = new ApplicationDbContext();
            await _dbContext.AddRangeAsync(transactionReports);
            return await _dbContext.SaveChangesAsync();
        }       

        public async Task<IList<TransactionReportEntity>> GetUnsoldCryptoTransactions(string cryptoName)
        {
            await using var _dbContext = new ApplicationDbContext();
            return await _dbContext.TransactionReports.Where(c =>
                c.Type.ToLower().Contains("Otwarta pozycja".ToLower())
                && c.Details.ToLower().Contains($"{cryptoName.ToLower()}/")
                && c.ClosedPosition == null).ToListAsync();
        }

        public async Task<IList<TransactionReportEntity>> GetDividendTransactions()
        {
            await using var _dbContext = new ApplicationDbContext();
            return await _dbContext.TransactionReports.Where(c =>
                c.Details.ToLower().Contains("Payment caused by dividend".ToLower())
                || c.Details.ToLower().Contains("Płatność w wyniku dywidendy".ToLower())).ToListAsync();
        }       
    }
}