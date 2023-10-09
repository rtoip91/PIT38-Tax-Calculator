using System.Threading;
using System.Threading.Tasks;

namespace TaxCalculatingService.Interfaces
{
    public interface IFileProcessor : IFileProcessingControl
    {
        Task ProcessFiles(CancellationToken token);
       
    }
}
