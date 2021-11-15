using System.Threading.Tasks;

namespace TaxEtoro.Interfaces
{
    interface ICalculator <in A> 
    {
        Task<T> Calculate<T>() where T : A;
    }
}
