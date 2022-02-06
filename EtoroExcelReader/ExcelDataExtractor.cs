﻿using AutoMapper;
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

namespace ExcelReader;

public class ExcelDataExtractor : IExcelDataExtractor
{
    private readonly IClosedPositionsDataAccess _closedPositionsDataAccess;
    private readonly ITransactionReportsDataAccess _transactionReportsDataAccess;

    public ExcelDataExtractor(IClosedPositionsDataAccess closedPositionsDataAccess,
        ITransactionReportsDataAccess transactionReportsDataAccess)
    {
        _closedPositionsDataAccess = closedPositionsDataAccess;
        _transactionReportsDataAccess = transactionReportsDataAccess;
    }

    public async Task<bool> ImportDataFromExcelIntoDbAsync()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        var filePath = FileInputUtil.GetFileInfo(@"..\\TestFile", "TestFile2021.xlsx").FullName;
        FileInfo fileInfo = new FileInfo(filePath);

        IList<ClosedPositionExcelDto> closedPositionDtos = new List<ClosedPositionExcelDto>();
        IList<TransactionReportExcelDto> transactionReportDtos = new List<TransactionReportExcelDto>();

        using ExcelPackage package = new ExcelPackage();
        await package.LoadAsync(fileInfo);

        DataTable closedPositionsDataTable = await CreateDataTableAsync(package, ExcelSpreadsheets.ClosedPositions);
        DataTable transactionReportsDataTable =
            await CreateDataTableAsync(package, ExcelSpreadsheets.TransactionReports);

        Task extractClosedPositions = ExtractClosedPositionsAsync(closedPositionsDataTable, closedPositionDtos);
        Task extractTransactionReports =
            ExtractTransactionReportsAsync(transactionReportsDataTable, transactionReportDtos);

        await Task.WhenAll(extractClosedPositions, extractTransactionReports);

        var result = await SaveIntoTheDatabaseAsync(closedPositionDtos, transactionReportDtos);

        return result;
    }

    private async Task<bool> SaveIntoTheDatabaseAsync(IList<ClosedPositionExcelDto> closedPositionDtos,
        IList<TransactionReportExcelDto> transactionReportDtos)
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ClosedPositionProfile>();
            cfg.AddProfile<TransactionReportProfile>();
        });

        var mapper = new Mapper(config);
        IList<TransactionReportEntity> transactionReportEntities = new List<TransactionReportEntity>();
        IList<ClosedPositionEntity> closedPositionEntities = new List<ClosedPositionEntity>();

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

        try
        {
            Task<int> addClosePositions = _closedPositionsDataAccess.AddClosePositions(closedPositionEntities);
            Task<int> addTransactionReports =
                _transactionReportsDataAccess.AddTransactionReports(transactionReportEntities);

            await Task.WhenAll(addClosePositions, addTransactionReports);
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