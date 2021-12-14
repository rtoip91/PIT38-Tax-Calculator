using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Entities;

namespace Database.DataAccess.Interfaces
{
    public interface IClosedPositionsDataAccess
    {
        Task<int> AddClosePositions(IList<ClosedPositionEntity> closedPositions);
        Task<IList<ClosedPositionEntity>> GetCfdPositions();
        Task<IList<ClosedPositionEntity>> GetStockPositions();
        Task<IList<ClosedPositionEntity>> GetCryptoPositions(string cryptoName);
        Task<int> RemovePosition(ClosedPositionEntity closedPosition);
    }
}
