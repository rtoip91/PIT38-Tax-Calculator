using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxEtoro.BussinessLogic
{
    internal interface ICfdCalculator
    {
        Task<bool> Calculate();
    }
}
