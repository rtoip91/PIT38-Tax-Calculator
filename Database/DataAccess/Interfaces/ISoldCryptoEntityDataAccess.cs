﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Entities;

namespace Database.DataAccess.Interfaces
{
    public interface ISoldCryptoEntityDataAccess
    { 
        void AddEntities(IList<SoldCryptoEntity> soldCryptoEntities);
        IList<SoldCryptoEntity> GetSoldCryptoEntities();
    }
}
