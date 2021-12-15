namespace Calculations.Interfaces
{
    internal interface ICalculator<in A>
    {
        Task<T> Calculate<T>() where T : A;
    }
}