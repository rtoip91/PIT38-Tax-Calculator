using System.Collections.Generic;
using System.Linq;
using Database.DataAccess.Interfaces;
using Database.Entities;
using Database.Repository;

namespace Database.DataAccess
{
    public class TransactionReportsDataAccess : ITransactionReportsDataAccess
    {
        private readonly IImportRepository _importRepository;
        public TransactionReportsDataAccess( IImportRepository importRepository)
        {
            _importRepository = importRepository;
        }
        public void AddTransactionReports(IList<TransactionReportEntity> transactionReports)
        {
            foreach (var transactionReport in transactionReports)
            {
                _importRepository.TransactionReports.Add(transactionReport);
            }
        }       

        public  IList<TransactionReportEntity> GetUnsoldCryptoTransactions(string cryptoName)
        {
            return _importRepository.TransactionReports.Where(c =>
                c.Type.ToLower().Contains("Otwarta pozycja".ToLower())
                && c.Details.ToLower().Contains($"{cryptoName.ToLower()}/")
                && c.ClosedPosition == null).ToList();
        }
    }
}