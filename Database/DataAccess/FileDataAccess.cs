using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.DataAccess.Interfaces;
using Database.Entities.Database;
using Database.Enums;
using Microsoft.EntityFrameworkCore;

namespace Database.DataAccess
{
    public class FileDataAccess : IFileDataAccess
    {
        public async Task<string> AddNewFile(Guid operationGuid)
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

        public async Task<string> GetCalculationResultFileName(Guid operationGuid)
        {
            await using var context = new ApplicationDbContext();

            var fileEntity = context.FileEntities.FirstOrDefault(f => f.OperationGuid == operationGuid);

            if (fileEntity is null)
            {
                return null;
            }

            return fileEntity.CalculationResultFileName;
        }

        public async Task<string> GetInputFileName(Guid operationGuid)
        {
            await using var context = new ApplicationDbContext();

            var fileEntity = context.FileEntities.FirstOrDefault(f => f.OperationGuid == operationGuid);

            if (fileEntity is null)
            {
                return null;
            }

            return fileEntity.InputFileName;
        }

        public async Task<bool> SetAsCalculated(Guid operationGuid, string calculationResultJson)
        {
            await using var context = new ApplicationDbContext();

            var fileEntity = context.FileEntities.FirstOrDefault(f => f.OperationGuid == operationGuid);
            if (fileEntity == null)
            {
                return false;
            }

            fileEntity.CalculationResultJson = calculationResultJson;

            fileEntity.Status = FileStatus.Calculated;
            fileEntity.StatusChangeDate = DateTime.UtcNow;

            await context.SaveChangesAsync();
            return true;
        }

        public async Task<IList<Guid>> GetOperationsToProcess()
        {
            await using var context = new ApplicationDbContext();
            return await context.FileEntities.Where(f => f.Status == FileStatus.Added).Select(f => f.OperationGuid)
                .ToListAsync();
        }

        public async Task<IList<string>> GetCalculationResultFilesToDelete()
        {
            
            await using var context = new ApplicationDbContext();
            IList<string> resultFilesToDelete = new List<string>();
            var now = DateTime.UtcNow;

            var downloadedFiles = context.FileEntities.Where(f => f.Status == FileStatus.Downloaded).ToList();
            var calculatedFiles = context.FileEntities.Where(f => f.Status == FileStatus.Calculated).ToList();

            foreach (var downloadedFile in downloadedFiles)
            {
                TimeSpan timeSpan = now - downloadedFile.StatusChangeDate;

                if( timeSpan.Days >= 1)
                {
                    resultFilesToDelete.Add(downloadedFile.CalculationResultFileName);
                }
            }

            foreach (var calculatedFile in calculatedFiles)
            {
                TimeSpan timeSpan = now - calculatedFile.StatusChangeDate;

                if (timeSpan.Days >= 2)
                {
                    resultFilesToDelete.Add(calculatedFile.CalculationResultFileName);
                }
            }

            return resultFilesToDelete;
        }

        public async Task<bool> SetAsDownloaded(Guid operationGuid)
        {
            await using var context = new ApplicationDbContext();

            var fileEntity = context.FileEntities.FirstOrDefault(f => f.OperationGuid == operationGuid);
            if (fileEntity == null)
            {
                return false;
            }           

            fileEntity.Status = FileStatus.Downloaded;
            fileEntity.StatusChangeDate = DateTime.UtcNow;

            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SetAsDeleted(string fileName)
        {
            await using var context = new ApplicationDbContext();

            var fileEntity = context.FileEntities.FirstOrDefault(f => f.CalculationResultFileName == fileName);
            if (fileEntity == null)
            {
                return false;
            }

            fileEntity.Status = FileStatus.Deleted;
            fileEntity.StatusChangeDate = DateTime.UtcNow;

            await context.SaveChangesAsync();
            return true;
        }
    }
}