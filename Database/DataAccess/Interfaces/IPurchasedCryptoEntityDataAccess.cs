using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Entities;
using Database.Entities.InMemory;

namespace Database.DataAccess.Interfaces
{
    public interface IPurchasedCryptoEntityDataAccess
    {
        void AddEntities(IList<PurchasedCryptoEntity> purchasedCryptoEntities);
        IList<PurchasedCryptoEntity> GetPurchasedCryptoEntities();
    }
}