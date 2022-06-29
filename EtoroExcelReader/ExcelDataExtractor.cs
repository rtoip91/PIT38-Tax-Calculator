using AutoMapper;
using Database.DataAccess.Interfaces;
using Database.Entities;
using EtoroExcelReader.Dto;
using ExcelReader.Dictionatries;
using ExcelReader.Interfaces;
using ExcelReader.MappingProfiles;
using OfficeOpenXml;
using OfficeOpenXml.Export.ToDataTable;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Database.Entities.InMemory;
using ExcelReader.Dto;

namespace ExcelReader;

public class ExcelDataExtractor : IExcelDataExtractor
{
    private readonly IClosedPositionsDataAccess _closedPositionsDataAccess;
    private readonly ITransactionReportsDataAccess _transactionReportsDataAccess;
    private readonly IDividendsDataAccess _dividendsDataAccess;

    public ExcelDataExtractor(IClosedPositionsDataAccess closedPositionsDataAccess,
        ITransactionReportsDataAccess transactionReportsDataAccess,
        IDividendsDataAccess dividendsDataAccess)
    {
        _closedPositionsDataAccess = closedPositionsDataAccess;
        _transactionReportsDataAccess = transactionReportsDataAccess;
        _dividendsDataAccess = dividendsDataAccess;
    }

    public async Task<bool> ImportDataFromExcel(string directory, string fileName)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        var filePath = FileInputUtil.GetFileInfo(directory, fileName).FullName;
        FileInfo fileInfo = new FileInfo(filePath);

        if (fileInfo.Extension != ".xlsx")
        {
            throw new Exception("Zły typ pliku.");
        }

        IList<ClosedPositionExcelDto> closedPositionDtos = new List<ClosedPositionExcelDto>();
        IList<TransactionReportExcelDto> transactionReportDtos = new List<TransactionReportExcelDto>();
        IList<DividendDto> dividendDtos = new List<DividendDto>();

        using ExcelPackage package = new ExcelPackage();
        await package.LoadAsync(fileInfo);

        DataTable closedPositionsDataTable = await CreateDataTableAsync(package, ExcelSpreadsheets.ClosedPositions);
        DataTable transactionReportsDataTable =
            await CreateDataTableAsync(package, ExcelSpreadsheets.TransactionReports);
        DataTable dividendsDataTable = await CreateDataTableAsync(package, ExcelSpreadsheets.Dividends);

        Task extractClosedPositions = ExtractClosedPositionsAsync(closedPositionsDataTable, closedPositionDtos);
        Task extractTransactionReports =
            ExtractTransactionReportsAsync(transactionReportsDataTable, transactionReportDtos);
        Task extractDividends = ExtractDividendsAsync(dividendsDataTable, dividendDtos);

        await Task.WhenAll(extractClosedPositions, extractTransactionReports, extractDividends);

        var result = await SaveIntoTheDatabaseAsync(closedPositionDtos, transactionReportDtos, dividendDtos);

        return result;
    }

    private async Task<bool> SaveIntoTheDatabaseAsync(IList<ClosedPositionExcelDto> closedPositionDtos,
        IList<TransactionReportExcelDto> transactionReportDtos, IList<DividendDto> dividendDtos)
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ClosedPositionProfile>();
            cfg.AddProfile<TransactionReportProfile>();
            cfg.AddProfile<DividendProfile>();
        });

        var mapper = new Mapper(config);
        IList<TransactionReportEntity> transactionReportEntities = new List<TransactionReportEntity>();
        IList<ClosedPositionEntity> closedPositionEntities = new List<ClosedPositionEntity>();
        IList<DividendEntity> dividendEntities = new List<DividendEntity>();

        foreach (ClosedPositionExcelDto closedPosition in closedPositionDtos)
        {
            ClosedPositionEntity closedPositionEntity = mapper.Map<ClosedPositionEntity>(closedPosition);
            closedPositionEntity.TransactionReports = new List<TransactionReportEntity>();


            foreach (TransactionReportExcelDto transactionReport in transactionReportDtos
                         .Where(t => t.PositionId == closedPosition.PositionId).ToList())
            {
                closedPositionEntity.TransactionReports.Add(mapper.Map<TransactionReportEntity>(transactionReport));
                transactionReportDtos.Remove(transactionReport);
            }

            closedPositionEntities.Add(closedPositionEntity);
        }

        foreach (TransactionReportExcelDto transactionReport in transactionReportDtos)
        {
            TransactionReportEntity transactionReportEntity =
                mapper.Map<TransactionReportEntity>(transactionReport);
            transactionReportEntity.PositionId = null;
            transactionReportEntities.Add(transactionReportEntity);
        }

        foreach (var dividendDto in dividendDtos)
        {
            dividendEntities.Add(mapper.Map<DividendEntity>(dividendDto));
        }

        try
        {
            _closedPositionsDataAccess.AddClosePositions(closedPositionEntities);
            _transactionReportsDataAccess.AddTransactionReports(transactionReportEntities);
            _dividendsDataAccess.AddDividends(dividendEntities);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }

        return true;
    }

    private async Task<DataTable> CreateDataTableAsync(ExcelPackage package, int worksheetId)
    {
        return await Task.Run((() =>
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets[worksheetId];

            string name = worksheet.Name.Trim();

            ExcelAddressBase dimension = worksheet.Dimension;
            int numberOfRows = dimension.Rows;
            int numberOfColumns = dimension.Columns;

            ToDataTableOptions options = ToDataTableOptions.Create();
            options.DataTableName = name;
            options.FirstRowIsColumnNames = true;


            DataTable dataTable = worksheet.Cells[1, 1, numberOfRows, numberOfColumns].ToDataTable(options);
            return dataTable;
        }));
    }

    private async Task ExtractClosedPositionsAsync(DataTable dataTable,
        IList<ClosedPositionExcelDto> closedPositionDtos)
    {
        await Task.Run(() =>
        {
            foreach (DataRow row in dataTable.Rows)
            {
                ClosedPositionExcelDto closedPosition = new ClosedPositionExcelDto(row);
                closedPositionDtos.Add(closedPosition);
            }
        });
    }

    private async Task ExtractDividendsAsync(DataTable dataTable,
        IList<DividendDto> dividendDtos)
    {
        await Task.Run(() =>
        {
            foreach (DataRow row in dataTable.Rows)
            {
                DividendDto closedPosition = new DividendDto(row);
                dividendDtos.Add(closedPosition);
            }
        });
    }

    private async Task ExtractTransactionReportsAsync(DataTable dataTable,
        IList<TransactionReportExcelDto> transactionReportDtos)
    {
        await Task.Run(() =>
        {
            foreach (DataRow row in dataTable.Rows)
            {
                TransactionReportExcelDto transactionReport = new TransactionReportExcelDto(row);
                transactionReportDtos.Add(transactionReport);
            }
        });
    }
}