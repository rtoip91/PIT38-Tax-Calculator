using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxEtoro.Interfaces
{
    public interface IFileCleaner
    {
        Task CleanCalculationResultFiles();
    }
}
