using System.Collections.Generic;
using System.Linq;
using Database.Entities.InMemory;
using ExcelReader.Dto;
using ExcelReader.Interfaces;

namespace ExcelReader
{
    internal sealed class ExtractedDataProcessor : IExtractedDataProcessor
    {
        public IList<ClosedPositionEntity> CreateClosedPositionEntitiesWithRelatedTransactionReports(
            ExtractedDataDto extractedData)
        {
            IList<ClosedPositionEntity> closedPositionEntities = new List<ClosedPositionEntity>();

            foreach (ClosedPositionEntity closedPositionEntity in extractedData.ClosedPositions)
            {
                closedPositionEntity.TransactionReports = new List<TransactionReportEntity>();
                foreach (TransactionReportEntity transactionReport in extractedData.TransactionReports
                             .Where(t => t.PositionId == closedPositionEntity.PositionId).ToList())
                {
                    closedPositionEntity.TransactionReports.Add(transactionReport);
                    extractedData.TransactionReports.Remove(transactionReport);
                }

                closedPositionEntities.Add(closedPositionEntity);
            }

            return closedPositionEntities;
        }

        public IList<TransactionReportEntity> CreateUnrelatedTransactionReportEntities(ExtractedDataDto extractedData)
        {
            IList<TransactionReportEntity> transactionReportEntities = new List<TransactionReportEntity>();

            foreach (TransactionReportEntity transactionReport in extractedData.TransactionReports)
            {
                TransactionReportEntity transactionReportEntity = transactionReport;
                transactionReportEntity.PositionId = null;
                transactionReportEntities.Add(transactionReportEntity);
            }

            return transactionReportEntities;
        }

        public IList<DividendEntity> CreateDividendEntities(ExtractedDataDto extractedDataDto)
        {
            IList<DividendEntity> dividendEntities = new List<DividendEntity>();

            foreach (var dividendDto in extractedDataDto.Dividends)
            {
                dividendEntities.Add(dividendDto);
            }

            return dividendEntities;
        }
    }
}