using System;
using System.Threading;
using System.Threading.Tasks;
using TaxCalculatingService.BussinessLogic;

namespace TaxEtoro.Interfaces
{
    public interface IFileProcessor : IObserver<FileUploadedEvent>
    {
        Task ProcessFiles(CancellationToken token);
    }
}
