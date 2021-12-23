using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.DataAccess.Interfaces;
using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database.DataAccess
{
    public class ClosedPositionsDataAccess : IClosedPositionsDataAccess
    {
        public async Task<int> AddClosePositions(IList<ClosedPositionEntity> closedPositions)
        {
            await using var context = new ApplicationDbContext();
            await context.AddRangeAsync(closedPositions);
            return await context.SaveChangesAsync();
        }

        public async Task<IList<ClosedPositionEntity>> GetCfdPositions()
        {
            await using var context = new ApplicationDbContext();
            return await context.ClosedPositions.Where(c => c.IsReal.Contains("CFD")).Include(c => c.TransactionReports)
                .ToListAsync();
        }
        public async Task<IList<ClosedPositionEntity>> GetCryptoPositions(IList<string> cryptoNames)
        {
            await using var context = new ApplicationDbContext();
            IList<ClosedPositionEntity> list = new List<ClosedPositionEntity>();
            foreach (string cryptoName in cryptoNames)
            {
                var temp = context.ClosedPositions.Where(c =>
                       c.Operation.ToLower().Contains($" {cryptoName.ToLower()}") && !c.IsReal.Contains("CFD"))
                    .Include(c => c.TransactionReports).ToList();
                if (temp.Any())
                {
                    list = list.Concat(temp).ToList();
                }
            }

            return list;
        }

        public async Task<IList<ClosedPositionEntity>> GetStockPositions()
        {
            await using var context = new ApplicationDbContext();
            return await context.ClosedPositions.Include(c => c.TransactionReports).ToListAsync();
        }

        public async Task<int> RemovePosition(ClosedPositionEntity closedPosition)
        {
            await using var context = new ApplicationDbContext();
            if (closedPosition.TransactionReports != null)
            {
                context.RemoveRange(closedPosition.TransactionReports);
            }

            context.Remove(closedPosition);
            return await context.SaveChangesAsync();
        }
    }
}