using System.Data;
using Database.Entities.InMemory;

namespace ExcelReader.Converters;

internal interface IRowToEntityConverter
{
    ClosedPositionEntity ToClosedPositionEntity(DataRow row);
    TransactionReportEntity ToTransactionReportEntity(DataRow row);
    DividendEntity ToDividendEntity(DataRow row);
}