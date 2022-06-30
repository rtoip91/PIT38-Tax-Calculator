using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.DataAccess.Interfaces
{
    public interface IFileDataAccess
    {
        Task<IList<Guid>> GetOperationsToProcess();
        Task<string> AddNewFile(Guid operationGuid);

        Task<string> GetCalculationResultFileName(Guid operationGuid);
        Task<string> GetInputFileName(Guid operationGuid);

        Task<bool> SetAsCalculated(Guid operationGuid, string calculationResultJson);
    }
}