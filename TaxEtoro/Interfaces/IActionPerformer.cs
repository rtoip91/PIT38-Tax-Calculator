using System.Threading.Tasks;

namespace TaxCalculatingService.Interfaces
{
    public interface IActionPerformer
    {
        public Task ClearResultFilesPeriodically();
    }

}