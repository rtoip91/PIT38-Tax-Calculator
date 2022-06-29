using System.Collections.Generic;
using System.Linq;
using Database.DataAccess.Interfaces;
using Database.Entities;
using Database.Entities.InMemory;
using Database.Repository;

namespace Database.DataAccess
{
    public class IncomeByCountryDataAccess : IIncomeByCountryDataAccess
    {
        private readonly IDataRepository _repository;

        public IncomeByCountryDataAccess(IDataRepository repository)
        {
            _repository = repository;
        }

        public void AddIncome(string countryName, decimal income)
        {
            var incomeByCountry = _repository.IncomeByCountryEntities.FirstOrDefault(i => i.Country == countryName);
            if (incomeByCountry != null)
            {
                incomeByCountry.Income += income;
            }
            else
            {
                var entity = new IncomeByCountryEntity { Country = countryName, Income = income, PaidTax = 0m };
                _repository.IncomeByCountryEntities.Add(entity);
            }
        }

        public IList<IncomeByCountryEntity> GetAllIncomes()
        {
            return _repository.IncomeByCountryEntities;
        }

        public IncomeByCountryEntity GetIncomeByCountryName(string countryName)
        {
            return _repository.IncomeByCountryEntities.FirstOrDefault(i => string.Equals(i.Country, countryName));
        }
    }
}