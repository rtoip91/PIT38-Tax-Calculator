using System.Collections.Generic;
using System.Threading.Tasks;
using Database.Entities;
using Database.Entities.InMemory;

namespace Database.DataAccess.Interfaces
{
    public interface IClosedPositionsDataAccess
    {
        int AddClosePositions(IList<ClosedPositionEntity> closedPositions);
        IList<ClosedPositionEntity> GetCfdPositions();
        IList<ClosedPositionEntity> GetStockPositions();
        IList<ClosedPositionEntity> GetCryptoPositions();
        void RemovePosition(ClosedPositionEntity closedPosition);
    }
}