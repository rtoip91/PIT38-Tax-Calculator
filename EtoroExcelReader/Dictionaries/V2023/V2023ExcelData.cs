﻿using System.Collections.Generic;
using ExcelReader.Dictionaries.Interfaces;

namespace ExcelReader.Dictionaries.V2023;

public sealed class V2023ExcelData : IExcelData
{
    public IReadOnlyDictionary<int, string> SpreadSheets { get; }
    public IReadOnlyDictionary<string, IList<string>> ColumnNames { get; }

    public V2023ExcelData()
    {
        List<string> closedPositions = new List<string>
        {
            "Identyfikator pozycji",
            "Operacja",
            "Kwota",
            "Jednostki",
            "Data otwarcia",
            "Data zamknięcia",
            "Dźwignia",
            "Opłaty w spreadzie (USD)",
            "Spread rynkowy (USD)",
            "Zysk (USD)",
            "Stawka na otwarciu",
            "Stawka na zamknięciu",
            "Kurs Take Profit",
            "Kurs Stop Loss",
            "Opłaty za rolowanie i dywidendy",
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
            "Zmiana zrealizowanego kapitału",
            "Kapitał zrealizowany",
            "Saldo",
            "Identyfikator pozycji",
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
            "Identyfikator pozycji",
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