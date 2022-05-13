using System.Collections.Generic;
using System.Threading.Tasks;
using Database.Entities;

namespace Database.DataAccess.Interfaces
{
    public interface IDividendsDataAccess
    {
        void AddDividends(IList<DividendEntity> dividends);
        IList<DividendEntity> GetDividends();
    }
}