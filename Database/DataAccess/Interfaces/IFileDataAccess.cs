﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Database.Entities.Database;
using Database.Enums;

namespace Database.DataAccess.Interfaces;

public interface IFileDataAccess
{
    List<Guid> GetOperationsToProcess();
    Task<List<Guid>> GetOperationsToProcessAsync();
    Task<int> GetOperationsToProcessNumberAsync();
    Task<string> AddNewFileAsync(Guid operationGuid, FileVersion fileVersion, MemoryStream fileContent);
    Task<string> GetCalculationResultFileNameAsync(Guid operationGuid);
    Task<FileEntity> GetInputFileDataAsync(Guid operationGuid);
    Task<bool> SetAsCalculatedAsync(Guid operationGuid, string calculationResultJson);
    Task<bool> SetAsDownloadedAsync(Guid operationGuid);
    Task<bool> SetAsDeletedAsync(string fileName);
    Task<bool> SetAsDeletedAsync(Guid operationGuid);
    Task<bool> SetAsInProgressAsync(Guid operationGuid);
    Task<IList<string>> GetCalculationResultFilesToDeleteAsync();
    Task<int> RemoveOldDataAboutDeletedFiles();
    Task<bool> RemoveFileContentAsync(string fileName);
    Task<bool> AddCalculationResultFileContentAsync(Guid operationGuid, MemoryStream resultFileContent);
    Task<MemoryStream> GetCalculationResultFileContentAsync(Guid operationGuid);
}