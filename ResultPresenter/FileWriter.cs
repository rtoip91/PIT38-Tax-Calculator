using Calculations.Dto;
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

    public async Task PresentData(CalculationResultDto calculationResultDto)
    {
        IList<CfdEntity> cfdEntities = await _cfdEntityDataAccess.GetCfdEntities();
        FileStream fs = new FileStream("result.txt", FileMode.Create, FileAccess.Write);
        await using StreamWriter sw = new StreamWriter(fs);
        await WriteCfdToFile(calculationResultDto, cfdEntities, sw);
    }

    private async Task WriteCfdToFile(CalculationResultDto calculationResultDto, IList<CfdEntity> cfdEntities, StreamWriter sw)
    {
        await sw.WriteLineAsync("--------CFD--------");
        await sw.WriteLineAsync($"Zysk = {calculationResultDto.CdfDto.Gain} PLN");
        await sw.WriteLineAsync($"Strata = {calculationResultDto.CdfDto.Loss} PLN");
        await sw.WriteLineAsync($"Dochód = {calculationResultDto.CdfDto.Income} PLN");
        await sw.WriteLineAsync($"\nIlość operacji: {cfdEntities.Count}\n");
        foreach (CfdEntity cfdEntity in cfdEntities)
        {
            await sw.WriteLineAsync(cfdEntity.ToString());
        }
    }
}
