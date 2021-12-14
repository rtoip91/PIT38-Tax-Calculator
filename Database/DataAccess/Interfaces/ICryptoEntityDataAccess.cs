using System.Collections.Generic;
using System.Threading.Tasks;
using Database.Entities;

namespace Database.DataAccess.Interfaces
{
    public interface ICryptoEntityDataAccess
    {
        Task<int> AddEntities(IList<CryptoEntity> cryptoEntities);
    }
}
