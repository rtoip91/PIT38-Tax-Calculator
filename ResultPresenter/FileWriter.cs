using Calculations.Dto;
using Database.DataAccess.Interfaces;
using Database.Entities;
using ResultsPresenter.Interfaces;

namespace ResultsPresenter;

public class FileWriter : IFileWriter
{
    private readonly ICryptoEntityDataAccess _cryptoEntityDataAccess;
    private readonly ICfdEntityDataAccess _cfdEntityDataAccess;
    private readonly IStockEntityDataAccess _stockEntityDataAccess;

    public FileWriter(ICryptoEntityDataAccess cryptoEntityDataAccess,
        ICfdEntityDataAccess cfdEntityDataAccess,
        IStockEntityDataAccess stockEntityDataAccess)
    {
        _cryptoEntityDataAccess = cryptoEntityDataAccess;
        _cfdEntityDataAccess = cfdEntityDataAccess;
        _stockEntityDataAccess = stockEntityDataAccess;
    }

    public async Task PresentData(CalculationResultDto calculationResultDto)
    {
        await WriteCfdResultsToFile(calculationResultDto.CdfDto);
        await WriteStockResultsToFile(calculationResultDto.StockDto);
    }

    private async Task WriteStockResultsToFile(StockCalculatorDto stockCalculatorDto)
    {
        FileStream fs = new FileStream("STOCK_result.txt", FileMode.Create, FileAccess.Write);
        await using StreamWriter sw = new StreamWriter(fs);

        IList<StockEntity> stockEntities = await _stockEntityDataAccess.GetEntities();

        await sw.WriteLineAsync("--------Akcje--------");
        await sw.WriteLineAsync($"Koszt = {stockCalculatorDto.Cost} PLN");
        await sw.WriteLineAsync($"Przychód = {stockCalculatorDto.Revenue} PLN");
        await sw.WriteLineAsync($"Dochód = {stockCalculatorDto.Income} PLN");
        await sw.WriteLineAsync($"\nIlość operacji: {stockEntities.Count}\n");

        foreach (StockEntity stockEntity in stockEntities)
        {
            await sw.WriteLineAsync(stockEntity.ToString());
        }
    }

    private async Task WriteCfdResultsToFile(CfdCalculatorDto cfdCalculatorDto)
    {
        IList<CfdEntity> cfdEntities = await _cfdEntityDataAccess.GetCfdEntities();
        FileStream fs = new FileStream("CFD_result.txt", FileMode.Create, FileAccess.Write);
        await using StreamWriter sw = new StreamWriter(fs);

        await sw.WriteLineAsync("--------CFD--------");
        await sw.WriteLineAsync($"Zysk = {cfdCalculatorDto.Gain} PLN");
        await sw.WriteLineAsync($"Strata = {cfdCalculatorDto.Loss} PLN");
        await sw.WriteLineAsync($"Dochód = {cfdCalculatorDto.Income} PLN");
        await sw.WriteLineAsync($"\nIlość operacji: {cfdEntities.Count}\n");

        foreach (CfdEntity cfdEntity in cfdEntities)
        {
            await sw.WriteLineAsync(cfdEntity.ToString());
        }
    }
}