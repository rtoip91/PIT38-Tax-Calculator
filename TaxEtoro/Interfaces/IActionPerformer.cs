using System;
using System.Threading.Tasks;

namespace TaxEtoro.Interfaces
{
    internal interface IActionPerformer : IAsyncDisposable
    {
        Task PerformCalculations();
        Task PresentCalcucaltionResults();
        void OnAppClose(object sender, EventArgs e);
    }
}