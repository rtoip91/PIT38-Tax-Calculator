using System;
using System.Threading;
using System.Threading.Tasks;
using TaxCalculatingService.BussinessLogic;

namespace TaxCalculatingService.Interfaces
{
    public interface IFileProcessor : IObserver<FileUploadedEvent>
    {
        Task ProcessFiles(CancellationToken token);
    }
}
