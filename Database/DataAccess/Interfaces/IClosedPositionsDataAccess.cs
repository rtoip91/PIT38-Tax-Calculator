using System.Collections.Generic;
using System.Threading.Tasks;
using Database.Entities;

namespace Database.DataAccess.Interfaces
{
    public interface IClosedPositionsDataAccess
    {
        Task<int> AddClosePositions(IList<ClosedPositionEntity> closedPositions);
        Task<IList<ClosedPositionEntity>> GetCfdPositions();
        Task<IList<ClosedPositionEntity>> GetStockPositions();
        Task<IList<ClosedPositionEntity>> GetCryptoPositions(IList<string> cryptoNames);
        Task<int> RemovePosition(ClosedPositionEntity closedPosition);
    }
}