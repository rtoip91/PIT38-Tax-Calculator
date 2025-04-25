using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using Database.Entities.InMemory;
using ExcelReader.Converters;
using ExcelReader.Dictionaries.V2021;
using ExcelReader.Dto;
using ExcelReader.Factory;
using ExcelReader.Interfaces;
using OfficeOpenXml;
using OfficeOpenXml.Export.ToDataTable;

namespace ExcelReader
{
    internal sealed class ExcelFileHandler : IExcelFileHandler
    {
        private readonly IRowToEntityConverter _converter;

        public ExcelFileHandler(IConverterFactory converterFactory)
        {
            _converter = converterFactory.GetConverter();
        }

        public async Task<ExtractedDataDto> ExtractDataFromExcel(MemoryStream fileContent)
        {
            ExcelPackage.License.SetNonCommercialPersonal("Rtoip91"); 

            try
            {
                using ExcelPackage package = new ExcelPackage();
                await package.LoadAsync(fileContent);

                ExtractedDataDto extractedDataDto = new ExtractedDataDto();

                DataTable closedPositionsDataTable =
                    await CreateDataTableAsync(package, ExcelSpreadsheetsV2021.ClosedPositions);
                DataTable transactionReportsDataTable =
                    await CreateDataTableAsync(package, ExcelSpreadsheetsV2021.TransactionReports);
                DataTable dividendsDataTable = await CreateDataTableAsync(package, ExcelSpreadsheetsV2021.Dividends);


                await ExtractClosedPositionsAsync(closedPositionsDataTable, extractedDataDto);
                await ExtractTransactionReportsAsync(transactionReportsDataTable, extractedDataDto);
                await ExtractDividendsAsync(dividendsDataTable, extractedDataDto);

                // await Task.WhenAll(extractClosedPositions, extractTransactionReports, extractDividends);
                return extractedDataDto;
            }
            catch (Exception)
            {
                throw new Exception("Wrong file content");
            }
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
            ExtractedDataDto extractedData)
        {
            await Task.Run(() =>
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    ClosedPositionEntity closedPosition = _converter.ToClosedPositionEntity(row);
                    extractedData.ClosedPositions.Add(closedPosition);
                }

                dataTable.Rows.Clear();
            });
        }

        private async Task ExtractDividendsAsync(DataTable dataTable,
            ExtractedDataDto extractedData)
        {
            await Task.Run(() =>
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    DividendEntity dividend = _converter.ToDividendEntity(row);
                    extractedData.Dividends.Add(dividend);
                }

                dataTable.Rows.Clear();
            });
        }

        private async Task ExtractTransactionReportsAsync(DataTable dataTable,
            ExtractedDataDto extractedData)
        {
            await Task.Run(() =>
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    TransactionReportEntity transactionReport = _converter.ToTransactionReportEntity(row);
                    extractedData.TransactionReports.Add(transactionReport);
                }

                dataTable.Rows.Clear();
            });
        }
    }
}