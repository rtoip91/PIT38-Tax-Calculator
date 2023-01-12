using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.DataAccess.Interfaces;
using Database.Entities.Database;
using Database.Enums;
using Microsoft.EntityFrameworkCore;

namespace Database.DataAccess;

public sealed class FileDataAccess : IFileDataAccess
{
    public async Task<string> AddNewFileAsync(Guid operationGuid)
    {
        await using var context = new ApplicationDbContext();

        var fileEntity = new FileEntity();
        fileEntity.OperationGuid = operationGuid;
        fileEntity.InputFileName = $"{fileEntity.OperationGuid}.xlsx";
        fileEntity.CalculationResultFileName = $"{fileEntity.OperationGuid}.zip";
        fileEntity.Status = FileStatus.Added;
        fileEntity.StatusChangeDate = DateTime.UtcNow;

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

    public async Task<string> GetInputFileNameAsync(Guid operationGuid)
    {
        await using var context = new ApplicationDbContext();

        FileEntity fileEntity = context.FileEntities.FirstOrDefault(f => f.OperationGuid == operationGuid);

        if (fileEntity is null) return null;

        return fileEntity.InputFileName;
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

    public async Task<IList<string>> GetCalculationResultFilesToDeleteAsync()
    {
        await using var context = new ApplicationDbContext();
        IList<string> resultFilesToDelete = new List<string>();
        DateTime now = DateTime.UtcNow;

        var downloadedFiles = context.FileEntities.AsParallel().Where(f => f.Status == FileStatus.Downloaded).ToList();
        var calculatedFiles = context.FileEntities.AsParallel().Where(f => f.Status == FileStatus.Calculated).ToList();

        foreach (FileEntity downloadedFile in downloadedFiles)
        {
            TimeSpan timeSpan = now - downloadedFile.StatusChangeDate;

            if (timeSpan.Days >= 1) resultFilesToDelete.Add(downloadedFile.CalculationResultFileName);
        }

        foreach (FileEntity calculatedFile in calculatedFiles)
        {
            TimeSpan timeSpan = now - calculatedFile.StatusChangeDate;

            if (timeSpan.Days >= 2) resultFilesToDelete.Add(calculatedFile.CalculationResultFileName);
        }

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