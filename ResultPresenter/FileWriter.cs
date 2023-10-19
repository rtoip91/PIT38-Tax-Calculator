using System.IO.Compression;
using Calculations.Dto;
using Database.DataAccess.Interfaces;
using Database.Entities.InMemory;
using Microsoft.Extensions.Configuration;
using ResultsPresenter.Interfaces;

namespace ResultsPresenter;

public sealed class FileWriter : IFileWriter
{
    private readonly ICfdEntityDataAccess _cfdEntityDataAccess;
    private readonly IStockEntityDataAccess _stockEntityDataAccess;
    private readonly ISoldCryptoEntityDataAccess _soldCryptoEntityDataAccess;
    private readonly IPurchasedCryptoEntityDataAccess _purchasedCryptoEntityDataAccess;
    private readonly IDividendCalculationsDataAccess _dividendCalculationsDataAccess;
    private readonly IIncomeByCountryDataAccess _incomeByCountryDataAccess;
    private readonly IFileDataAccess _fileDataAccess;
    private readonly string _filePath;
    private string _fileName;
    private Guid _operationGuid;

    public FileWriter(ICfdEntityDataAccess cfdEntityDataAccess,
        IStockEntityDataAccess stockEntityDataAccess,
        ISoldCryptoEntityDataAccess soldCryptoEntityDataAccess,
        IPurchasedCryptoEntityDataAccess purchasedCryptoEntityDataAccess,
        IDividendCalculationsDataAccess dividendCalculationsDataAccess,
        IIncomeByCountryDataAccess incomeByCountryDataAccess,
        IFileDataAccess fileDataAccess,
        IConfiguration configuration)
    {
        _cfdEntityDataAccess = cfdEntityDataAccess;
        _stockEntityDataAccess = stockEntityDataAccess;
        _soldCryptoEntityDataAccess = soldCryptoEntityDataAccess;
        _purchasedCryptoEntityDataAccess = purchasedCryptoEntityDataAccess;
        _dividendCalculationsDataAccess = dividendCalculationsDataAccess;
        _incomeByCountryDataAccess = incomeByCountryDataAccess;
        _fileDataAccess = fileDataAccess;
        _filePath = configuration.GetValue<string>("ResultStorageFolder");
    }

    public async Task<MemoryStream> PresentData(Guid operationId,  MemoryStream inputFileContent,
        CalculationResultDto calculationResultDto)
    {
        CreateDirectory();
        _operationGuid = operationId;

        MemoryStream zipFile = new MemoryStream();
        using var archive = new ZipArchive(zipFile, ZipArchiveMode.Update);

        await WriteCfdResultsToFile(calculationResultDto.CdfDto, archive);
        await WriteStockResultsToFile(calculationResultDto.StockDto, archive);
        await WriteCryptoResultsToFile(calculationResultDto.CryptoDto, archive);
        await WriteDividendResultsToFile(calculationResultDto.DividendDto, archive);
        await WritePitZgToFile(archive);
        await CopyExcelFileToZip(inputFileContent, archive);
        return zipFile;
    }

    private async Task<string> GetFileName()
    {
        if (string.IsNullOrEmpty(_fileName))
        {
            _fileName = await _fileDataAccess.GetCalculationResultFileNameAsync(_operationGuid);
        }

        return _fileName;
    }

    private void CreateDirectory()
    {
        var path = $"{_filePath}";

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    private async Task<FileStream> CreateOrUpdateZipFile()
    {
        var fileName = await GetFileName();
        var path = $"{_filePath}/{fileName}";

        var zipToOpen = new FileStream(path, FileMode.OpenOrCreate);

        return zipToOpen;
    }

    private Task CopyExcelFileToZip(MemoryStream inputFileContent, ZipArchive archive)
    {
        var entry = archive.CreateEntry(Constants.Constants.EtoroExcelFile, CompressionLevel.Fastest);
        using (var entryStream = entry.Open())
        {
            inputFileContent.CopyTo(entryStream);
        }
        
        return Task.CompletedTask;
    }

    private async Task WritePitZgToFile(ZipArchive archive)
    {
        ZipArchiveEntry pitZgEntry = CreateZipFileEntry(archive, Constants.Constants.PitZgFileName);
        await using var sw = new StreamWriter(pitZgEntry.Open());

        IList<IncomeByCountryEntity> incomeByCountryEntities = _incomeByCountryDataAccess.GetAllIncomes();

        await sw.WriteLineAsync("--------Pit ZG--------");

        foreach (IncomeByCountryEntity income in incomeByCountryEntities.Where(i => i.Income > 0))
            await sw.WriteLineAsync(income.ToString());
    }

    private ZipArchiveEntry CreateZipFileEntry(ZipArchive archive, string entryName)
    {
        ZipArchiveEntry? entry = archive.GetEntry(entryName);
        entry?.Delete();
        ZipArchiveEntry pitZgEntry = archive.CreateEntry(entryName);
        return pitZgEntry;
    }


    private async Task WriteStockResultsToFile(StockCalculatorDto? stockCalculatorDto, ZipArchive archive)
    {
        ZipArchiveEntry stockEntry = CreateZipFileEntry(archive, Constants.Constants.StockCalculationsFileName);
        await using var sw = new StreamWriter(stockEntry.Open());

        IList<StockEntity> stockEntities = _stockEntityDataAccess.GetEntities();

        await sw.WriteLineAsync("--------Akcje i ETFy--------");
        await sw.WriteLineAsync($"Koszt = {stockCalculatorDto.Cost} PLN");
        await sw.WriteLineAsync($"Przychód = {stockCalculatorDto.Revenue} PLN");
        await sw.WriteLineAsync($"Dochód = {stockCalculatorDto.Income} PLN");
        await sw.WriteLineAsync($"\nIlość operacji: {stockEntities.Count}\n");

        foreach (StockEntity stockEntity in stockEntities) await sw.WriteLineAsync(stockEntity.ToString());
    }

    private async Task WriteCryptoResultsToFile(CryptoDto? cryptoDto, ZipArchive archive)
    {
        ZipArchiveEntry cryptoEntry = CreateZipFileEntry(archive, Constants.Constants.CryptoCalculationsFileName);
        await using var sw = new StreamWriter(cryptoEntry.Open());

        IList<PurchasedCryptoEntity> purchasedCryptoEntities =
            _purchasedCryptoEntityDataAccess.GetPurchasedCryptoEntities();
        IList<SoldCryptoEntity> soldCryptoEntities = _soldCryptoEntityDataAccess.GetSoldCryptoEntities();

        var operationNumber = purchasedCryptoEntities.Count + soldCryptoEntities.Count;

        await sw.WriteLineAsync("--------Kryptowaluty--------");
        await sw.WriteLineAsync($"Koszt zakupu = {cryptoDto.Cost} PLN");
        await sw.WriteLineAsync($"Przychód = {cryptoDto.Revenue} PLN");
        await sw.WriteLineAsync($"Dochód = {cryptoDto.Income} PLN");
        await sw.WriteLineAsync($"\nIlość operacji: {operationNumber}\n");
        await sw.WriteLineAsync("\n--------KUPIONE--------\n");

        foreach (PurchasedCryptoEntity purchasedCryptoEntity in purchasedCryptoEntities)
            await sw.WriteLineAsync(purchasedCryptoEntity.ToString());

        await sw.WriteLineAsync("\n--------SPRZEDANE--------\n");

        foreach (SoldCryptoEntity soldCryptoEntity in soldCryptoEntities)
            await sw.WriteLineAsync(soldCryptoEntity.ToString());
    }

    private async Task WriteDividendResultsToFile(DividendCalculatorDto? dividendCalculatorDto, ZipArchive archive)
    {
        ZipArchiveEntry dividendsEntry = CreateZipFileEntry(archive, Constants.Constants.DividendsCalculationsFileName);
        await using var sw = new StreamWriter(dividendsEntry.Open());
        IList<DividendCalculationsEntity> dividendCalculations = _dividendCalculationsDataAccess.GetEntities();

        await sw.WriteLineAsync("--------Dywidendy--------");
        await sw.WriteLineAsync($"Wartość dywidend = {dividendCalculatorDto.Dividend}");
        await sw.WriteLineAsync($"Podatek zapłacony ={dividendCalculatorDto.TaxPaid} ");
        await sw.WriteLineAsync($"Podatek pozostały do zaplaty ={dividendCalculatorDto.TaxToBePaid}");
        await sw.WriteLineAsync($"\nIlość dywidend: {dividendCalculations.Count}\n");

        foreach (DividendCalculationsEntity dividend in dividendCalculations)
            await sw.WriteLineAsync(dividend.ToString());
    }

    private async Task WriteCfdResultsToFile(CfdCalculatorDto? cfdCalculatorDto, ZipArchive archive)
    {
        IList<CfdEntity> cfdEntities = _cfdEntityDataAccess.GetCfdEntities();
        ZipArchiveEntry cfdEntry = CreateZipFileEntry(archive, Constants.Constants.CfdCalculationsFileName);

        await using var sw = new StreamWriter(cfdEntry.Open());

        await sw.WriteLineAsync("--------CFD--------");
        await sw.WriteLineAsync($"Zysk = {cfdCalculatorDto.Gain} PLN");
        await sw.WriteLineAsync($"Strata = {cfdCalculatorDto.Loss} PLN");
        await sw.WriteLineAsync($"Dochód = {cfdCalculatorDto.Income} PLN");
        await sw.WriteLineAsync($"\nIlość operacji: {cfdEntities.Count}\n");

        foreach (CfdEntity cfdEntity in cfdEntities) await sw.WriteLineAsync(cfdEntity.ToString());
    }
}