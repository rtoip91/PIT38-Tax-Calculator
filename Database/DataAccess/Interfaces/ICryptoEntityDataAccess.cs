using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Entities;

namespace Database.DataAccess.Interfaces
{
    public interface ICryptoEntityDataAccess
    {
        Task<int> AddEntities(IList<CryptoEntity> cryptoEntities);
    }
}
