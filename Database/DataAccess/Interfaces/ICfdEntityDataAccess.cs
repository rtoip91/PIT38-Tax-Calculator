using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Entities;

namespace Database.DataAccess.Interfaces
{
    public interface ICfdEntityDataAccess
    {
        Task<int> AddEntities(IList<CfdEntity> cfdEntities);
        Task<IList<CfdEntity>> GetCfdEntities();
    }
}