using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.DataAccess.Interfaces;
using Database.Entities;
using Database.Repository;
using Microsoft.EntityFrameworkCore;

namespace Database.DataAccess
{
    public class ClosedPositionsDataAccess : IClosedPositionsDataAccess
    {
        private readonly IImportRepository _importRepository;

        public ClosedPositionsDataAccess(IImportRepository importRepository)
        {
            _importRepository = importRepository;
        }

        public int AddClosePositions(IList<ClosedPositionEntity> closedPositions)
        {
            foreach (var closedPosition in closedPositions)
            {
                _importRepository.ClosedPositions.Add(closedPosition);
            }

            return closedPositions.Count;
        }

        public IList<ClosedPositionEntity> GetCfdPositions()
        {
            return _importRepository.ClosedPositions.Where(c => c.IsReal.Contains("CFD")).ToList();
        }

        public IList<ClosedPositionEntity> GetCryptoPositions()
        {
            return _importRepository.ClosedPositions.Where(c => c.IsReal.Contains("Kryptoaktywa")).ToList();
        }

        public IList<ClosedPositionEntity> GetStockPositions()
        {
            return _importRepository.ClosedPositions.Where(c => c.IsReal.Contains("Akcje") || c.IsReal.Contains("ETF"))
                .ToList();
        }

        public void RemovePosition(ClosedPositionEntity closedPosition)
        {
            _importRepository.ClosedPositions.Remove(closedPosition);
        }
    }
}