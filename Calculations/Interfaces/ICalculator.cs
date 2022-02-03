namespace Calculations.Interfaces
{
    public interface ICalculator<in A>
    {
        Task<T> Calculate<T>() where T : A;
    }
}