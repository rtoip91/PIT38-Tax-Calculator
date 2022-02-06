using Database.DataAccess.Interfaces;
using Database.Entities;
using ResultPresenter.Interfaces;

namespace ResultPresenter;

public class FileWriter : IFileWriter
{

    private readonly ICryptoEntityDataAccess _cryptoEntityDataAccess;
    private readonly ICfdEntityDataAccess _cfdEntityDataAccess;

    public FileWriter(ICryptoEntityDataAccess cryptoEntityDataAccess,
        ICfdEntityDataAccess cfdEntityDataAccess)
    {
        _cryptoEntityDataAccess = cryptoEntityDataAccess;
        _cfdEntityDataAccess = cfdEntityDataAccess;
    }

    public async Task PresentData()
    {
        IList<CfdEntity> cfdEntities = await _cfdEntityDataAccess.GetCfdEntities();

        foreach (CfdEntity cfdEntity in cfdEntities)
        {
            Console.WriteLine(cfdEntity.ToString());
        }

    }
}
