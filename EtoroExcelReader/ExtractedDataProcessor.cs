using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Database.Entities.InMemory;
using EtoroExcelReader.Dto;
using ExcelReader.Dto;
using ExcelReader.Interfaces;
using ExcelReader.MappingProfiles;

namespace ExcelReader
{
    internal class ExtractedDataProcessor : IExtractedDataProcessor
    {
        private readonly Mapper _mapper;

        public ExtractedDataProcessor()
        {
            _mapper = ConfigureMapper();
        }

        public IList<ClosedPositionEntity> CreateClosedPositionEntitiesWithRelatedTransactionReports(ExtractedDataDto extractedData)
        {
            IList<ClosedPositionEntity> closedPositionEntities = new List<ClosedPositionEntity>();

            foreach (ClosedPositionExcelDto closedPosition in extractedData.ClosedPositions)
            {
                ClosedPositionEntity closedPositionEntity = _mapper.Map<ClosedPositionEntity>(closedPosition);
                closedPositionEntity.TransactionReports = new List<TransactionReportEntity>();


                foreach (TransactionReportExcelDto transactionReport in extractedData.TransactionReports
                             .Where(t => t.PositionId == closedPosition.PositionId).ToList())
                {
                    closedPositionEntity.TransactionReports.Add(_mapper.Map<TransactionReportEntity>(transactionReport));
                    extractedData.TransactionReports.Remove(transactionReport);
                }

                closedPositionEntities.Add(closedPositionEntity);
            }

            return closedPositionEntities;
        }

        public IList<TransactionReportEntity> CreateUnrelatedTransactionReportEntities(ExtractedDataDto extractedData)
        {

            IList<TransactionReportEntity> transactionReportEntities = new List<TransactionReportEntity>();

            foreach (TransactionReportExcelDto transactionReport in extractedData.TransactionReports)
            {
                TransactionReportEntity transactionReportEntity = _mapper.Map<TransactionReportEntity>(transactionReport);
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
                dividendEntities.Add(_mapper.Map<DividendEntity>(dividendDto));
            }

            return dividendEntities;
        }

        private Mapper ConfigureMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ClosedPositionProfile>();
                cfg.AddProfile<TransactionReportProfile>();
                cfg.AddProfile<DividendProfile>();
            });

            return new Mapper(config);
        }
    }
}
