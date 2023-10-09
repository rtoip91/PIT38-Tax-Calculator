using System;
using System.Threading;
using System.Threading.Tasks;
using TaxCalculatingService.BussinessLogic;

namespace TaxCalculatingService.Interfaces
{
    public interface IFileProcessor
    {
        Task ProcessFiles(CancellationToken token);
    }
}
