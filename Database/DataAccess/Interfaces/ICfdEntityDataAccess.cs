using System.Collections.Generic;
using Database.Entities;
using Database.Entities.InMemory;

namespace Database.DataAccess.Interfaces
{
    public interface ICfdEntityDataAccess
    {
        void AddEntities(IList<CfdEntity> cfdEntities);
        IList<CfdEntity> GetCfdEntities();
    }
}