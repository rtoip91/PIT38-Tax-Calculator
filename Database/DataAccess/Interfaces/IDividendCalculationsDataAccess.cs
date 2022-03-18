using System.Collections.Generic;
using System.Threading.Tasks;
using Database.Entities;

namespace Database.DataAccess.Interfaces
{
    public  interface IDividendCalculationsDataAccess
    {
        void AddEntities(IList<DividendCalculationsEntity> dividendCalculationsEntities);
        IList<DividendCalculationsEntity> GetEntities();
    }
}
