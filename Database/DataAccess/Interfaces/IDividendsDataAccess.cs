using System.Collections.Generic;
using System.Threading.Tasks;
using Database.Entities;

namespace Database.DataAccess.Interfaces
{
    public interface IDividendsDataAccess
    {
        Task<int> AddDividends(IList<DividendEntity> dividends);

        Task<IList<DividendEntity>> GetDividends();
    }
}
