using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database;
using Database.Entities;

namespace TaxEtoro.BussinessLogic
{
    class CfdCalculator : ICfdCalculator
    {
        public async Task<bool> Calculate()
        {
            using (var context = new ApplicationDbContext())
            {
                var cfdClosedPositions = context.ClosedPositions.Where(c => c.IsReal == "CFD");
                IList<CfdEntity> cfdEntities = new List<CfdEntity>();

                foreach (var cfdClosedPosition in cfdClosedPositions)
                {
                    CfdEntity cfdEntity = new CfdEntity
                    {
                        Name = cfdClosedPosition.Operation,
                        PurchaseDate = cfdClosedPosition.OpeningDate,
                        OpeningRate = cfdClosedPosition.OpeningRate ?? 0,
                        ClosingRate = cfdClosedPosition.ClosingRate ?? 0,
                        SellDate = cfdClosedPosition.ClosingDate,
                        Units = cfdClosedPosition.Units ?? 0,
                        CurrencySymbol = "USD",
                        GainValue = cfdClosedPosition.Profit ?? 0
                    };
                    
                    cfdEntities.Add(cfdEntity);

                    if (cfdClosedPosition.TransactionReports != null)
                    {
                        context.RemoveRange(cfdClosedPosition.TransactionReports);
                    }

                    context.Remove(cfdClosedPosition);
                }

                await context.AddRangeAsync(cfdEntities);

                try
                {
                    await context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return false;
                }
            }

            return true;
        }
    }
}
