using System.Collections.Generic;
using Database.Entities.InMemory;
using ExcelReader.Dto;

namespace ExcelReader.Interfaces
{
    /// <summary>
    /// Interface for processing extracted data.
    /// </summary>
    public interface IExtractedDataProcessor
    {
        /// <summary>
        /// Creates a list of ClosedPositionEntity objects with related TransactionReports from the extracted data.
        /// </summary>
        /// <param name="extractedData">The data extracted from an Excel file.</param>
        /// <returns>A list of ClosedPositionEntity objects.</returns>
        IList<ClosedPositionEntity> CreateClosedPositionEntitiesWithRelatedTransactionReports(ExtractedDataDto extractedData);

        /// <summary>
        /// Creates a list of TransactionReportEntity objects that are not related to any ClosedPositionEntity from the extracted data.
        /// </summary>
        /// <param name="extractedData">The data extracted from an Excel file.</param>
        /// <returns>A list of TransactionReportEntity objects.</returns>
        IList<TransactionReportEntity> CreateUnrelatedTransactionReportEntities(ExtractedDataDto extractedData);

        /// <summary>
        /// Creates a list of DividendEntity objects from the extracted data.
        /// </summary>
        /// <param name="extractedDataDto">The data extracted from an Excel file.</param>
        /// <returns>A list of DividendEntity objects.</returns>
        IList<DividendEntity> CreateDividendEntities(ExtractedDataDto extractedDataDto);
    }
}