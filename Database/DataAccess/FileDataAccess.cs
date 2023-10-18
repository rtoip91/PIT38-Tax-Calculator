using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Database.DataAccess.Interfaces;
using Database.Entities.Database;
using Database.Enums;
using Microsoft.EntityFrameworkCore;

namespace Database.DataAccess;

public sealed class FileDataAccess : IFileDataAccess
{
    public async Task<string> AddNewFileAsync(Guid operationGuid, FileVersion fileVersion,  MemoryStream fileContent)
    {
        await using var context = new ApplicationDbContext();

        var fileEntity = new FileEntity();
        fileEntity.OperationGuid = operationGuid;
        fileEntity.InputFileName = $"{fileEntity.OperationGuid}.xlsx";
        fileEntity.CalculationResultFileName = $"{fileEntity.OperationGuid}.zip";
        fileEntity.Status = FileStatus.Added;
        fileEntity.StatusChangeDate = DateTime.UtcNow;
        fileEntity.FileVersion = fileVersion;
        fileEntity.InputFileContent = fileContent.ToArray();

        await context.FileEntities.AddAsync(fileEntity);
        await context.SaveChangesAsync();

        return fileEntity.InputFileName;
    }

    public async Task<string> GetCalculationResultFileNameAsync(Guid operationGuid)
    {
        await using var context = new ApplicationDbContext();

        FileEntity fileEntity = context.FileEntities.FirstOrDefault(f => f.OperationGuid == operationGuid);

        if (fileEntity is null) return null;

        return fileEntity.CalculationResultFileName;
    }

    public async Task<FileEntity> GetInputFileDataAsync(Guid operationGuid)
    {
        await using var context = new ApplicationDbContext();

        FileEntity fileEntity = context.FileEntities.FirstOrDefault(f => f.OperationGuid == operationGuid);

        if (fileEntity is null) return null;

        return fileEntity;
    }

    public async Task<bool> SetAsCalculatedAsync(Guid operationGuid, string calculationResultJson)
    {
        await using var context = new ApplicationDbContext();

        FileEntity fileEntity =
            context.FileEntities.FirstOrDefault(f =>
                f.OperationGuid == operationGuid && f.Status == FileStatus.InProgress);
        if (fileEntity == null) return false;

        fileEntity.CalculationResultJson = calculationResultJson;

        fileEntity.Status = FileStatus.Calculated;
        fileEntity.StatusChangeDate = DateTime.UtcNow;

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Guid>> GetOperationsToProcessAsync()
    {
        await using var context = new ApplicationDbContext();
        return context.FileEntities.AsParallel().Where(f => f.Status == FileStatus.Added).Take(100)
            .Select(f => f.OperationGuid).ToList();
    }


    public async Task<int> GetOperationsToProcessNumberAsync()
    {
        await using var context = new ApplicationDbContext();
        return await context.FileEntities.CountAsync(f => f.Status == FileStatus.Added);
    }

    public List<Guid> GetOperationsToProcess()
    {
        using var context = new ApplicationDbContext();
        return context.FileEntities.Where(f => f.Status == FileStatus.Added).Take(100).Select(f => f.OperationGuid)
            .ToList();
    }


    public async Task<IList<Guid>> GetOperationToProcess()
    {
        await using var context = new ApplicationDbContext();
        return await context.FileEntities.Where(f => f.Status == FileStatus.Added).Take(100)
            .Select(f => f.OperationGuid)
            .ToListAsync();
    }

    public async Task<int> RemoveOldDataAboutDeletedFiles()
    {
        await using var context = new ApplicationDbContext();
        var filesToDelete = context.FileEntities.AsParallel().Where(f =>
            f.Status == FileStatus.Deleted && 
            f.StatusChangeDate <= DateTime.UtcNow.Date.AddDays(-7)).ToList();

        context.FileEntities.RemoveRange(filesToDelete);
        return await context.SaveChangesAsync();
    }
    
    
    public async Task<IList<string>> GetCalculationResultFilesToDeleteAsync()
    {
        await using var context = new ApplicationDbContext();

        var resultFilesToDelete =  context.FileEntities.AsParallel().Where(f => 
            (f.Status == FileStatus.Downloaded && f.StatusChangeDate <=  DateTime.UtcNow.Date.AddDays(-1))
            || (f.Status == FileStatus.Calculated && f.StatusChangeDate <=  DateTime.UtcNow.Date.AddDays(-3)))
            .Select(f=>f.CalculationResultFileName).ToList();

        return resultFilesToDelete;
    }

    public async Task<bool> SetAsDownloadedAsync(Guid operationGuid)
    {
        await using var context = new ApplicationDbContext();

        FileEntity fileEntity = context.FileEntities.FirstOrDefault(f => f.OperationGuid == operationGuid);
        if (fileEntity == null) return false;

        fileEntity.Status = FileStatus.Downloaded;
        fileEntity.StatusChangeDate = DateTime.UtcNow;

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> SetAsDeletedAsync(string fileName)
    {
        await using var context = new ApplicationDbContext();

        FileEntity fileEntity = context.FileEntities.FirstOrDefault(f => f.CalculationResultFileName == fileName);
        if (fileEntity == null) return false;

        fileEntity.Status = FileStatus.Deleted;
        fileEntity.StatusChangeDate = DateTime.UtcNow;
        fileEntity.InputFileContent = null;

        await context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> RemoveFileContentAsync(string fileName)
    {
        await using var context = new ApplicationDbContext();

        FileEntity fileEntity = context.FileEntities.FirstOrDefault(f => f.InputFileName == fileName);
        if (fileEntity == null)
        {
            return false;
        }
       
        fileEntity.InputFileContent = null;

        await context.SaveChangesAsync();
        return true;
    }


    public async Task<bool> SetAsDeletedAsync(Guid operationGuid)
    {
        await using var context = new ApplicationDbContext();

        FileEntity fileEntity = context.FileEntities.FirstOrDefault(f => f.OperationGuid == operationGuid);
        if (fileEntity == null) return false;

        fileEntity.Status = FileStatus.Deleted;
        fileEntity.StatusChangeDate = DateTime.UtcNow;
        fileEntity.InputFileContent = null;

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> SetAsInProgressAsync(Guid operationGuid)
    {
        await using var context = new ApplicationDbContext();

        FileEntity fileEntity = context.FileEntities.FirstOrDefault(f => f.OperationGuid == operationGuid);
        if (fileEntity == null) return false;

        fileEntity.Status = FileStatus.InProgress;
        fileEntity.StatusChangeDate = DateTime.UtcNow;

        await context.SaveChangesAsync();
        return true;
    }
}