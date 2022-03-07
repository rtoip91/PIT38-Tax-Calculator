using Calculations.Dto;
using Database.DataAccess.Interfaces;
using Database.Entities;
using ResultPresenter.Interfaces;

namespace ResultsPresenter;

public class FileWriter : IFileWriter
{

   
    private readonly ICfdEntityDataAccess _cfdEntityDataAccess;
    private readonly IStockEntityDataAccess _stockEntityDataAccess;

    public FileWriter(ICfdEntityDataAccess cfdEntityDataAccess,
        IStockEntityDataAccess stockEntityDataAccess)
    {
        _cfdEntityDataAccess = cfdEntityDataAccess;
        _stockEntityDataAccess = stockEntityDataAccess;
    }

    public async Task PresentData(CalculationResultDto calculationResultDto)
    {
        CreateDirectory();
        await WriteCfdResultsToFile(calculationResultDto.CdfDto);
        await WriteStockResultsToFile(calculationResultDto.StockDto);
    }

    private void CreateDirectory()
    {
        if (!Directory.Exists(Constants.Constants.FilePath))
        {
            Directory.CreateDirectory(Constants.Constants.FilePath);
        }
    }

    private async Task WriteStockResultsToFile(StockCalculatorDto stockCalculatorDto)
    {
       
        FileStream fs = new FileStream($"{Constants.Constants.FilePath}{Constants.Constants.StockCalculationsFileName}", FileMode.Create, FileAccess.Write);
        await using StreamWriter sw = new StreamWriter(fs);

        IList<StockEntity> stockEntities = await _stockEntityDataAccess.GetEntities();

        await sw.WriteLineAsync("--------Akcje--------");
        await sw.WriteLineAsync($"Koszt zakupu = {stockCalculatorDto.Cost} PLN");
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
        FileStream fs = new FileStream($"{Constants.Constants.FilePath}{Constants.Constants.CfdCalculationsFileName}", FileMode.Create, FileAccess.Write);
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
