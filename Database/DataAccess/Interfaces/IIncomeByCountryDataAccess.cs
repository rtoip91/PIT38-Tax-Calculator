using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Entities;
using Database.Entities.InMemory;

namespace Database.DataAccess.Interfaces
{
    public interface IIncomeByCountryDataAccess
    {
        public void AddIncome(string countryName, decimal income);

        public IList<IncomeByCountryEntity> GetAllIncomes();

        public IncomeByCountryEntity GetIncomeByCountryName(string countryName);
    }
}