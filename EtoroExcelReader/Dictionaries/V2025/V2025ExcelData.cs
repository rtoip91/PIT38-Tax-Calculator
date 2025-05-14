using System.Collections.Generic;
using ExcelReader.Dictionaries.Interfaces;

namespace ExcelReader.Dictionaries.V2025;

public sealed class V2025ExcelData : IExcelData
{
    public IReadOnlyDictionary<int, string> SpreadSheets { get; }
    public IReadOnlyDictionary<string, IList<string>> ColumnNames { get; }

    public V2025ExcelData()
    {
        List<string> closedPositions = new List<string>
        {
            "ID pozycji",
            "Działanie",
            "Long / Short",
            "Kwota",
            "Jednostki",
            "Data otwarcia",
            "Data zamknięcia",
            "Dźwignia",
            "Opłaty za spreadzie (USD)",
            "Spread rynkowy (USD)",
            "Zysk (USD)",
            "Kurs walutowy przy otwarciu (USD)",
            "Kurs walutowy przy zamknięciu (USD)",
            "Kurs otwarcia",
            "Kurs zamknięcia",
            "Kurs take profit",
            "Kurs stop loss",
            "Opłaty typu overnight i dywidendy",
            "Skopiowano od",
            "Rodzaj",
            "ISIN",
            "Uwagi"
        };
        
        List<string> transactionReports = new List<string>
        {
            "Data",
            "Rodzaj",
            "Szczegóły",
            "Kwota",
            "Jednostki",
            "Zmiana zrealizowanego kapitału własnego",
            "Kapitał zrealizowany",
            "Saldo",
            "ID pozycji",
            "Typ aktywa",
            "Kwota niepodl. wypłacie"
        };
        
        List<string> dividends = new List<string>
        {
            "Data płatności",
            "Nazwa instrumentu",
            "Otrzymana dywidenda netto (USD)",
            "Stawka podatku u źródła (%)",
            "Kwota podatku u źródła (USD)",
            "ID pozycji",
            "Rodzaj",
            "ISIN"
        };
        
        SpreadSheets = new Dictionary<int, string>
        {
            { 0, "Podsumowanie rachunku" },
            { 1, "Pozycje zamknięte" },
            { 2, "Aktywność na rachunku" },
            { 3, "Dywidendy" },
            { 4, "Podsumowanie finansowe" }
        };
        
        ColumnNames = new Dictionary<string, IList<string>>
        {
            { "Podsumowanie rachunku", null },
            { "Pozycje zamknięte", closedPositions },
            { "Aktywność na rachunku", transactionReports },
            { "Dywidendy", dividends },
            { "Podsumowanie finansowe", null }
        };
    }
}