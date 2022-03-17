using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Entities;

namespace Database.Repository
{
    internal class ImportRepository : IImportRepository
    {
        public ImportRepository()
        {
            IncomeByCountryEntities = new List<IncomeByCountryEntity>();
        }
        public IList<IncomeByCountryEntity> IncomeByCountryEntities { get; set; }
       
    }
}
