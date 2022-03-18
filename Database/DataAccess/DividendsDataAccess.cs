using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.DataAccess.Interfaces;
using Database.Entities;
using Database.Repository;
using Microsoft.EntityFrameworkCore;

namespace Database.DataAccess
{
    public  class DividendsDataAccess : IDividendsDataAccess
    {
        private readonly IDataRepository _dataRepository;

        public DividendsDataAccess(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        public void AddDividends(IList<DividendEntity> dividends)
        {
            foreach (var dividend in dividends)
            {
                _dataRepository.Dividends.Add(dividend);
            }

        }

        public IList<DividendEntity> GetDividends()
        {
            return _dataRepository.Dividends;
        }
    }
}
