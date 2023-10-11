using System.Threading;
using System.Threading.Tasks;

namespace TaxCalculatingService.Interfaces
{
    public interface IFileProcessor
    {
        Task ProcessFiles(CancellationToken token);
       
    }
}
