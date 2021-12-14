using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Entities;

namespace Database.DataAccess.Interfaces
{
    public  interface ITransactionReportsDataAccess
    {
        Task<int> AddTransactionReports(IList<TransactionReportEntity> transactionReports);
        Task<IList<TransactionReportEntity>> GetUnsoldCryptoTransactions(string cryptoName);
        Task<IList<TransactionReportEntity>> GetDividendTransactions();
    }
}
