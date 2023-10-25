using Database.DataAccess.Interfaces;
using ExcelReader.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Database.Entities.InMemory;
using ExcelReader.Dto;
using Microsoft.Extensions.Logging;

namespace ExcelReader;

public sealed class ExcelDataExtractor : IExcelDataExtractor
{
    private readonly IExtractedDataProcessor _extractedDataProcessor;
    private readonly IExcelFileHandler _excelFileHandler;
    private readonly IClosedPositionsDataAccess _closedPositionsDataAccess;
    private readonly ITransactionReportsDataAccess _transactionReportsDataAccess;
    private readonly IDividendsDataAccess _dividendsDataAccess;
    private readonly ILogger<ExcelDataExtractor> _logger;

    public ExcelDataExtractor(IClosedPositionsDataAccess closedPositionsDataAccess,
        ITransactionReportsDataAccess transactionReportsDataAccess,
        IDividendsDataAccess dividendsDataAccess,
        IExcelFileHandler excelFileHandler,
        IExtractedDataProcessor extractedDataProcessor,
        ILogger<ExcelDataExtractor> logger)
    {
        _closedPositionsDataAccess = closedPositionsDataAccess;
        _transactionReportsDataAccess = transactionReportsDataAccess;
        _dividendsDataAccess = dividendsDataAccess;
        _logger = logger;
        _excelFileHandler = excelFileHandler;
        _extractedDataProcessor = extractedDataProcessor;
    }

    public async Task<bool> ImportDataFromExcel(MemoryStream fileContent)
    {
        var extractedData = await _excelFileHandler.ExtractDataFromExcel(fileContent);
        var result = await SaveExtractedData(extractedData);

        return result;
    }

    private Task<bool> SaveExtractedData(ExtractedDataDto extractedData)
    {
        IList<ClosedPositionEntity> closedPositionEntities =
            _extractedDataProcessor.CreateClosedPositionEntitiesWithRelatedTransactionReports(extractedData);
        IList<TransactionReportEntity> transactionReportEntities =
            _extractedDataProcessor.CreateUnrelatedTransactionReportEntities(extractedData);
        IList<DividendEntity> dividendEntities = _extractedDataProcessor.CreateDividendEntities(extractedData);

        try
        {
            _closedPositionsDataAccess.AddClosePositions(closedPositionEntities);
            _transactionReportsDataAccess.AddTransactionReports(transactionReportEntities);
            _dividendsDataAccess.AddDividends(dividendEntities);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return Task.FromResult(false);
        }

        return Task.FromResult(true);
    }
}