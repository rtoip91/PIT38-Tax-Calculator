using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Entities;

namespace Database.DataAccess.Interfaces
{
    public interface IPurchasedCryptoEntityDataAccess
    {
        Task<int> AddEntities(IList<PurchasedCryptoEntity> purchasedCryptoEntities);
        Task<IList<PurchasedCryptoEntity>> GetPurchasedCryptoEntities();
    }
}
