using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Database.DataAccess.Interfaces;

public interface IFileDataAccess
{
    List<Guid> GetOperationsToProcess();
    Task<List<Guid>> GetOperationsToProcessAsync();
    Task<int> GetOperationsToProcessNumberAsync();
    Task<string> AddNewFileAsync(Guid operationGuid);
    Task<string> GetCalculationResultFileNameAsync(Guid operationGuid);
    Task<string> GetInputFileNameAsync(Guid operationGuid);
    Task<bool> SetAsCalculatedAsync(Guid operationGuid, string calculationResultJson);
    Task<bool> SetAsDownloadedAsync(Guid operationGuid);
    Task<bool> SetAsDeletedAsync(string fileName);
    Task<bool> SetAsDeletedAsync(Guid operationGuid);
    Task<bool> SetAsInProgressAsync(Guid operationGuid);
    Task<IList<string>> GetCalculationResultFilesToDeleteAsync();
}