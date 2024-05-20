namespace Calculations.Interfaces
{
    /// <summary>
    /// Interface for a calculator that performs calculations on objects of type A.
    /// </summary>
    /// <typeparam name="A">The type of object the calculator operates on.</typeparam>
    public interface ICalculator<in A>
    {
        /// <summary>
        /// Performs a calculation and returns the result.
        /// </summary>
        /// <typeparam name="T">The type of the result. Must be a type that A can be assigned to.</typeparam>
        /// <returns>A Task representing the asynchronous operation. The result of the Task is the result of the calculation, or null if the calculation could not be performed.</returns>
        Task<T?> Calculate<T>() where T : A;
    }
}