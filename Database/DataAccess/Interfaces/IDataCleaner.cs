using System.Threading.Tasks;

namespace Database.DataAccess.Interfaces
{
    public interface IDataCleaner
    {
        Task CleanData();
    }
}