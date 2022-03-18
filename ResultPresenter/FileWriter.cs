using Calculations.Dto;
using Database.DataAccess.Interfaces;
using Database.Entities;
using ResultsPresenter.Interfaces;

namespace ResultsPresenter;

public class FileWriter : IFileWriter
{
    private readonly ICfdEntityDataAccess _cfdEntityDataAccess;
    private readonly IStockEntityDataAccess _stockEntityDataAccess;
    private readonly ISoldCryptoEntityDataAccess _soldCryptoEntityDataAccess;
    private readonly IPurchasedCryptoEntityDataAccess _purchasedCryptoEntityDataAccess;
    private readonly IDividendCalculationsDataAccess _dividendCalculationsDataAccess;
    private readonly IIncomeByCountryDataAccess _incomeByCountryDataAccess;

    public FileWriter(ICfdEntityDataAccess cfdEntityDataAccess,
        IStockEntityDataAccess stockEntityDataAccess,
        ISoldCryptoEntityDataAccess soldCryptoEntityDataAccess,
        IPurchasedCryptoEntityDataAccess purchasedCryptoEntityDataAccess,
        IDividendCalculationsDataAccess dividendCalculationsDataAccess,
        IIncomeByCountryDataAccess incomeByCountryDataAccess)
    {
        _cfdEntityDataAccess = cfdEntityDataAccess;
        _stockEntityDataAccess = stockEntityDataAccess;
        _soldCryptoEntityDataAccess = soldCryptoEntityDataAccess;
        _purchasedCryptoEntityDataAccess = purchasedCryptoEntityDataAccess;
        _dividendCalculationsDataAccess = dividendCalculationsDataAccess;
        _incomeByCountryDataAccess = incomeByCountryDataAccess;
    }

    public async Task PresentData(CalculationResultDto calculationResultDto)
    {
        CreateDirectory();
        await WriteCfdResultsToFile(calculationResultDto.CdfDto);
        await WriteStockResultsToFile(calculationResultDto.StockDto);
        await WriteCryptoResultsToFile(calculationResultDto.CryptoDto);
        await WriteDividendResultsToFile(calculationResultDto.DividendDto);
        await WritePitZgToFile();
    }

    private void CreateDirectory()
    {
        if (!Directory.Exists(Constants.Constants.FilePath))
        {
            Directory.CreateDirectory(Constants.Constants.FilePath);
        }
    }


    private async Task WritePitZgToFile()
    {
        FileStream fs = new FileStream($"{Constants.Constants.FilePath}{Constants.Constants.PitZgFileName}",
            FileMode.Create, FileAccess.Write);
        await using StreamWriter sw = new StreamWriter(fs);

        IList<IncomeByCountryEntity> incomeByCountryEntities = _incomeByCountryDataAccess.GetAllIncomes();

        await sw.WriteLineAsync("--------Pit ZG--------");

        foreach (var income in incomeByCountryEntities.Where(i=>i.Income > 0))
        {
            await sw.WriteLineAsync(income.ToString());
        }
    }


    private async Task WriteStockResultsToFile(StockCalculatorDto stockCalculatorDto)
    {
        FileStream fs = new FileStream($"{Constants.Constants.FilePath}{Constants.Constants.StockCalculationsFileName}",
            FileMode.Create, FileAccess.Write);
        await using StreamWriter sw = new StreamWriter(fs);

        IList<StockEntity> stockEntities = _stockEntityDataAccess.GetEntities();

        await sw.WriteLineAsync("--------Akcje i ETFy--------");
        await sw.WriteLineAsync($"Koszt = {stockCalculatorDto.Cost} PLN");
        await sw.WriteLineAsync($"Przychód = {stockCalculatorDto.Revenue} PLN");
        await sw.WriteLineAsync($"Dochód = {stockCalculatorDto.Income} PLN");
        await sw.WriteLineAsync($"\nIlość operacji: {stockEntities.Count}\n");

        foreach (StockEntity stockEntity in stockEntities)
        {
            await sw.WriteLineAsync(stockEntity.ToString());
        }
    }

    private async Task WriteCryptoResultsToFile(CryptoDto cryptoDto)
    {
        FileStream fs =
            new FileStream($"{Constants.Constants.FilePath}{Constants.Constants.CryptoCalculationsFileName}",
                FileMode.Create, FileAccess.Write);
        await using StreamWriter sw = new StreamWriter(fs);

        IList<PurchasedCryptoEntity> purchasedCryptoEntities = _purchasedCryptoEntityDataAccess.GetPurchasedCryptoEntities();
        IList<SoldCryptoEntity> soldCryptoEntities = _soldCryptoEntityDataAccess.GetSoldCryptoEntities();

        int operationNumber = purchasedCryptoEntities.Count + soldCryptoEntities.Count;

        await sw.WriteLineAsync("--------Kryptowaluty--------");
        await sw.WriteLineAsync($"Koszt zakupu = {cryptoDto.Cost} PLN");
        await sw.WriteLineAsync($"Przychód = {cryptoDto.Revenue} PLN");
        await sw.WriteLineAsync($"Dochód = {cryptoDto.Income} PLN");
        await sw.WriteLineAsync($"\nIlość operacji: {operationNumber}\n");
        await sw.WriteLineAsync("\n--------KUPIONE--------\n");

        foreach (PurchasedCryptoEntity purchasedCryptoEntity in purchasedCryptoEntities)
        {
            await sw.WriteLineAsync(purchasedCryptoEntity.ToString());
        }

        await sw.WriteLineAsync("\n--------SPRZEDANE--------\n");

        foreach (SoldCryptoEntity soldCryptoEntity in soldCryptoEntities)
        {
            await sw.WriteLineAsync(soldCryptoEntity.ToString());
        }
    }

    private async Task WriteDividendResultsToFile(DividendCalculatorDto dividendCalculatorDto)
    {
        FileStream fs =
            new FileStream($"{Constants.Constants.FilePath}{Constants.Constants.DividendsCalculationsFileName}",
                FileMode.Create, FileAccess.Write);
        await using StreamWriter sw = new StreamWriter(fs);

        IList<DividendCalculationsEntity> dividendCalculations = _dividendCalculationsDataAccess.GetEntities();

        await sw.WriteLineAsync("--------Dywidendy--------");
        await sw.WriteLineAsync($"Wartość dywidend = {dividendCalculatorDto.Dividend}");
        await sw.WriteLineAsync($"Podatek zapłacony ={dividendCalculatorDto.TaxPaid} ");
        await sw.WriteLineAsync($"Podatek pozostały do zaplaty ={dividendCalculatorDto.TaxToBePaid}");
        await sw.WriteLineAsync($"\nIlość dywidend: {dividendCalculations.Count}\n");

        foreach (var dividend in dividendCalculations)
        {
            await sw.WriteLineAsync(dividend.ToString());
        }
    }

    private async Task WriteCfdResultsToFile(CfdCalculatorDto cfdCalculatorDto)
    {
        IList<CfdEntity> cfdEntities = _cfdEntityDataAccess.GetCfdEntities();
        FileStream fs = new FileStream($"{Constants.Constants.FilePath}{Constants.Constants.CfdCalculationsFileName}",
            FileMode.Create, FileAccess.Write);
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