using System.Threading.Tasks;
using Database.Entities.Database;

namespace Database.DataAccess.Interfaces;

public interface ICryptocurrencyDataAccess
{
    Task<bool> SaveCryptocurrency(CryptocurrencyEntity cryptocurrencyEntity);
    Task<string> GetName(string code);
}