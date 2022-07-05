using System.Collections.Generic;
using Database.Entities.InMemory;
using ExcelReader.Dto;

namespace ExcelReader.Interfaces
{
    public interface IExtractedDataProcessor
    {
        IList<ClosedPositionEntity> CreateClosedPositionEntitiesWithRelatedTransactionReports(ExtractedDataDto extractedData);

        IList<TransactionReportEntity> CreateUnrelatedTransactionReportEntities(ExtractedDataDto extractedData);

        IList<DividendEntity> CreateDividendEntities(ExtractedDataDto extractedDataDto);
    }
}
