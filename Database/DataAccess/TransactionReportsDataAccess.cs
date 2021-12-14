using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.DataAccess.Interfaces;
using Database.Entities;

namespace Database.DataAccess
{
    public class TransactionReportsDataAccess : ITransactionReportsDataAccess, IDisposable
    {
        private readonly ApplicationDbContext _dbContext;

        public TransactionReportsDataAccess()
        {
            _dbContext = new ApplicationDbContext();
        }

        public async Task<int> AddTransactionReports(IList<TransactionReportEntity> transactionReports)
        {
           await _dbContext.AddRangeAsync(transactionReports);
            return await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<IList<TransactionReportEntity>> GetUnsoldCryptoTransactions(string cryptoName)
        {
            return _dbContext.TransactionReports.Where(c =>
                c.Type.ToLower().Contains("Otwarta pozycja".ToLower())
                && c.Details.ToLower().Contains($"{cryptoName.ToLower()}/")
                && c.ClosedPosition == null).ToList();
        }

        public async Task<IList<TransactionReportEntity>> GetDividendTransactions()
        {
            return _dbContext.TransactionReports.Where(c =>
                c.Details.ToLower().Contains("Payment caused by dividend".ToLower())
                || c.Details.ToLower().Contains("Płatność w wyniku dywidendy".ToLower())).ToList();
        }

        ~TransactionReportsDataAccess()
        {
            Dispose();
        }
    }
}
