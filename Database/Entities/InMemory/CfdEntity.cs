﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Database.Enums;

namespace Database.Entities.InMemory
{
    [Table("CfdCalculations")]
    public record CfdEntity
    {
        [Key] public int Id { get; set; }

        public TransactionType TransactionType { get; set; }
        public string Name { get; set; }
        public long PositionId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime SellDate { get; set; }
        public decimal Units { get; set; }
        public decimal OpeningRate { get; set; }
        public decimal ClosingRate { get; set; }
        public decimal GainValue { get; set; }
        public string CurrencySymbol { get; set; }
        public decimal ExchangeRate { get; set; }
        public DateTime ExchangeRateDate { get; set; }
        public decimal GainExchangedValue { get; set; }
        public decimal LossExchangedValue { get; set; }
        public int Leverage { get; set; }
        public string Country { get; set; }

        public override string ToString()
        {
            var exchangedGain = GainExchangedValue != 0 ? GainExchangedValue : LossExchangedValue;
            return $"Operacja: {Name} | ID: {PositionId} | Kraj: {Country} |" +
                   $"\nIlość jednostek: {Units} | Dźwignia: {Leverage} |" +
                   $"\nData otwarcia: {PurchaseDate.ToShortDateString()} | Cena za jednostkę: {OpeningRate} {CurrencySymbol} |" +
                   $"\nData zamknięcia: {SellDate.ToShortDateString()} | Cena za jednostkę: {ClosingRate} {CurrencySymbol} |" +
                   $"\nWynik w dniu zamknięcia: {GainValue} {CurrencySymbol} | Kurs {CurrencySymbol} z dnia {ExchangeRateDate.ToShortDateString()}: {ExchangeRate} PLN |" +
                   $"\nWynik po przeliczeniu: {exchangedGain} PLN\n";
        }
    }
}