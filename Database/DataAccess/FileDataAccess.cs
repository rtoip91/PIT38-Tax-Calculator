﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Database.DataAccess.Interfaces;
using Database.Entities.Database;
using Database.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Database.DataAccess;

public sealed class FileDataAccess : IFileDataAccess
{
    private readonly IServiceScopeFactory _scopeFactory;

    public FileDataAccess(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task<string> AddNewFileAsync(Guid operationGuid, FileVersion fileVersion, MemoryStream fileContent)
    {
        var fileEntity = new FileEntity();
        fileEntity.OperationGuid = operationGuid;
        fileEntity.InputFileName = $"{fileEntity.OperationGuid}.xlsx";
        fileEntity.CalculationResultFileName = $"{fileEntity.OperationGuid}.zip";
        fileEntity.Status = FileStatus.Added;
        fileEntity.StatusChangeDate = DateTime.UtcNow;
        fileEntity.FileVersion = fileVersion;

        InputFileContentEntity fileContentEntity = new InputFileContentEntity
        {
            FileContent = fileContent.ToArray()
        };

        fileEntity.InputFileContent = fileContentEntity;

        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await context.FileEntities.AddAsync(fileEntity);
        await context.SaveChangesAsync();

        return fileEntity.InputFileName;
    }

    public async Task<string> GetCalculationResultFileNameAsync(Guid operationGuid)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        FileEntity fileEntity = context.FileEntities.FirstOrDefault(f => f.OperationGuid == operationGuid);

        if (fileEntity is null) return null;

        return fileEntity.CalculationResultFileName;
    }

    public async Task<FileEntity> GetInputFileDataAsync(Guid operationGuid)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        FileEntity fileEntity = context.FileEntities.Include(f => f.InputFileContent)
            .FirstOrDefault(f => f.OperationGuid == operationGuid);

        if (fileEntity is null) return null;

        return fileEntity;
    }

    public async Task<bool> SetAsCalculatedAsync(Guid operationGuid, string calculationResultJson)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
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
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        return context.FileEntities.AsParallel().Where(f => f.Status == FileStatus.Added).Take(100)
            .Select(f => f.OperationGuid).ToList();
    }


    public async Task<int> GetOperationsToProcessNumberAsync()
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        return await context.FileEntities.CountAsync(f => f.Status == FileStatus.Added);
    }

    public List<Guid> GetOperationsToProcess()
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        return context.FileEntities.Where(f => f.Status == FileStatus.Added).Take(100).Select(f => f.OperationGuid)
            .ToList();
    }


    public async Task<IList<Guid>> GetOperationToProcess()
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        return await context.FileEntities.Where(f => f.Status == FileStatus.Added).Take(100)
            .Select(f => f.OperationGuid)
            .ToListAsync();
    }

    public async Task<int> RemoveOldDataAboutDeletedFiles()
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var filesToDelete = context.FileEntities.AsParallel().Where(f =>
            f.Status == FileStatus.Deleted &&
            f.StatusChangeDate <= DateTime.UtcNow.Date.AddDays(-7)).ToList();

        context.FileEntities.RemoveRange(filesToDelete);
        return await context.SaveChangesAsync();
    }


    public async Task<IList<string>> GetCalculationResultFilesToDeleteAsync()
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var resultFilesToDelete = context.FileEntities.AsParallel().Where(f =>
                (f.Status == FileStatus.Downloaded && f.StatusChangeDate <= DateTime.UtcNow.Date.AddDays(-1))
                || (f.Status == FileStatus.Calculated && f.StatusChangeDate <= DateTime.UtcNow.Date.AddDays(-3)))
            .Select(f => f.CalculationResultFileName).ToList();

        return resultFilesToDelete;
    }

    public async Task<bool> SetAsDownloadedAsync(Guid operationGuid)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        FileEntity fileEntity = context.FileEntities.FirstOrDefault(f => f.OperationGuid == operationGuid);
        if (fileEntity == null) return false;

        fileEntity.Status = FileStatus.Downloaded;
        fileEntity.StatusChangeDate = DateTime.UtcNow;

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> SetAsDeletedAsync(string fileName)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        FileEntity fileEntity = context.FileEntities.Include(fileEntity => fileEntity.CalculationResultFileContent)
            .FirstOrDefault(f => f.CalculationResultFileName == fileName);
        if (fileEntity == null) return false;

        fileEntity.Status = FileStatus.Deleted;
        fileEntity.StatusChangeDate = DateTime.UtcNow;
        context.Remove(fileEntity.CalculationResultFileContent);

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveFileContentAsync(string fileName)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        FileEntity fileEntity = context.FileEntities.Include(fileEntity => fileEntity.InputFileContent)
            .FirstOrDefault(f => f.InputFileName == fileName);
        if (fileEntity == null)
        {
            return false;
        }

        context.Remove(fileEntity.InputFileContent);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AddCalculationResultFileContentAsync(Guid operationGuid, MemoryStream resultFileContent)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        FileEntity fileEntity = context.FileEntities.FirstOrDefault(f => f.OperationGuid == operationGuid);
        if (fileEntity == null) return false;

        ResultFileContentEntity fileContentEntity = new ResultFileContentEntity
        {
            FileContent = resultFileContent.ToArray(),
        };

        fileEntity.CalculationResultFileContent = fileContentEntity;

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<MemoryStream> GetCalculationResultFileContentAsync(Guid operationGuid)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        FileEntity fileEntity = context.FileEntities.Include(fileEntity => fileEntity.CalculationResultFileContent)
            .FirstOrDefault(f => f.OperationGuid == operationGuid);
        if (fileEntity == null) return null;
        if (fileEntity.CalculationResultFileContent == null) return null;
        return new MemoryStream(fileEntity.CalculationResultFileContent.FileContent);
    }


    public async Task<bool> SetAsDeletedAsync(Guid operationGuid)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
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
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        FileEntity fileEntity = context.FileEntities.FirstOrDefault(f => f.OperationGuid == operationGuid);
        if (fileEntity == null) return false;

        fileEntity.Status = FileStatus.InProgress;
        fileEntity.StatusChangeDate = DateTime.UtcNow;

        await context.SaveChangesAsync();
        return true;
    }
}