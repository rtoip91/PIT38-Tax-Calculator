using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Database.DataAccess.Interfaces;
using Database.Entities.Database;
using Database.Enums;
using Database.Repository;
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
    }
}