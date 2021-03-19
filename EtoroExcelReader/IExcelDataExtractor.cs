using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Database.Entities;
using EtoroExcelReader.Dto;
using ExcelReader.Interfaces;
using OfficeOpenXml;
using OfficeOpenXml.Export.ToDataTable;

namespace ExcelReader
{
    public class ExcelDataExtractor : IExcelDataExtractor
    {
        private const int ClosedPositions = 1;
        private const int TransactionReports = 2;
        
        public async Task<bool> ImportDataFromExcelIntoDbAsync()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var filePath = FileInputUtil.GetFileInfo(@"C:\Etoro", "eToroAccountStatement - rtoip91 - 01-01-2020 - 31-12-2020.xlsx").FullName;
            FileInfo fileInfo = new FileInfo(filePath);

            IList<ClosedPositionExcelDto> closedPositionDtos = new List<ClosedPositionExcelDto>();
            IList<TransactionReportExcelDto> transactionReportDtos = new List<TransactionReportExcelDto>();

          
            using (ExcelPackage package = new ExcelPackage())
            {
                await package.LoadAsync(fileInfo);

                DataTable closedPositionsDataTable = await CreateDataTableAsync(package, ClosedPositions);
                DataTable transactionReportsDataTable = await CreateDataTableAsync(package, TransactionReports);

                IList<Task> extractingTasks = new List<Task>
                {
                    ExtractClosedPositionsAsync(closedPositionsDataTable, closedPositionDtos),
                    ExtractTransactionReportsAsync(transactionReportsDataTable, transactionReportDtos)
                };


                await Task.WhenAll(extractingTasks);

               return await IntoTheDatabaseAsync(closedPositionDtos, transactionReportDtos);
            }
        }

        private async Task<bool> IntoTheDatabaseAsync(IList<ClosedPositionExcelDto> closedPositionDtos, IList<TransactionReportExcelDto> transactionReportDtos)
        {
            using (var context = new ApplicationDbContext())
            {
                ClosedPositionExcelDto closedPosition = closedPositionDtos.First();
                TransactionReportExcelDto transactionReport = transactionReportDtos.First(a => a.PositionId == closedPosition.PositionId);


                TransactionReportEntity transactionReportEntity = new TransactionReportEntity
                {
                    PositionId = transactionReport.PositionId,
                    Amount = transactionReport.Amount,
                    Date = transactionReport.Date
                };

                ClosedPositionEntity closedPositionEntity = new ClosedPositionEntity
                {
                    Amount = closedPosition.Amount,
                    PositionId = closedPosition.PositionId,
                    Operation = closedPosition.Operation,
                    TransactionReports = new List<TransactionReportEntity> {transactionReportEntity}
                };

                await context.AddAsync<ClosedPositionEntity>(closedPositionEntity);

                try
                {
                    await context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return false;
                }
               

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

        private async Task ExtractClosedPositionsAsync(DataTable dataTable, IList<ClosedPositionExcelDto> closedPositionDtos)
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
}
