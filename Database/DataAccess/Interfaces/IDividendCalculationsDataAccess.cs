using System.Collections.Generic;
using System.Threading.Tasks;
using Database.Entities;

namespace Database.DataAccess.Interfaces
{
    public  interface IDividendCalculationsDataAccess
    {
        Task<int> AddEntities(IList<DividendCalculationsEntity> dividendCalculationsEntities);
        Task<IList<DividendCalculationsEntity>> GetEntities();
    }
}
