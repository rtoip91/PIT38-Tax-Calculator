using System.Threading;
using System.Threading.Tasks;

namespace TaxCalculatingService.Interfaces
{
    /// <summary>
    /// Interface for processing files.
    /// </summary>
    public interface IFileProcessor
    {
        /// <summary>
        /// Processes files asynchronously.
        /// </summary>
        /// <param name="token">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        Task ProcessFiles(CancellationToken token);
    }
}