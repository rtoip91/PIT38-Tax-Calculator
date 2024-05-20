using Calculations.Dto;

namespace Calculations.Interfaces
{
    /// <summary>
    /// Interface for performing tax calculations.
    /// </summary>
    public interface ITaxCalculations
    {
        /// <summary>
        /// Performs tax calculations and returns the result.
        /// </summary>
        /// <returns>A Task representing the asynchronous operation. The result of the Task is a CalculationResultDto containing the result of the tax calculations, or null if the calculations could not be performed.</returns>
        public Task<CalculationResultDto?> CalculateTaxes();
    }
}