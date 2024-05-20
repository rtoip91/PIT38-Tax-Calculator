using System.Data;
using Database.Entities.InMemory;

namespace ExcelReader.Converters
{
    /// <summary>
    /// Interface for converting DataRow objects to specific entity types.
    /// </summary>
    internal interface IRowToEntityConverter
    {
        /// <summary>
        /// Converts a DataRow to a ClosedPositionEntity.
        /// </summary>
        /// <param name="row">The DataRow to convert.</param>
        /// <returns>A ClosedPositionEntity that represents the DataRow.</returns>
        ClosedPositionEntity ToClosedPositionEntity(DataRow row);

        /// <summary>
        /// Converts a DataRow to a TransactionReportEntity.
        /// </summary>
        /// <param name="row">The DataRow to convert.</param>
        /// <returns>A TransactionReportEntity that represents the DataRow.</returns>
        TransactionReportEntity ToTransactionReportEntity(DataRow row);

        /// <summary>
        /// Converts a DataRow to a DividendEntity.
        /// </summary>
        /// <param name="row">The DataRow to convert.</param>
        /// <returns>A DividendEntity that represents the DataRow.</returns>
        DividendEntity ToDividendEntity(DataRow row);
    }
}