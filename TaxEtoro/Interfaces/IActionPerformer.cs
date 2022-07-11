using System.Threading.Tasks;

namespace TaxEtoro.Interfaces
{
    public interface IActionPerformer
    {
        public Task PerformCalculationsAndWriteResultsPeriodically();
        public Task ClearResultFilesPeriodically();
    }

}