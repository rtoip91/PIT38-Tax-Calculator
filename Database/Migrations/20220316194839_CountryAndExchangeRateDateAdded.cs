using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    public partial class CountryAndExchangeRateDateAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Comments",
                table: "ClosedPositions",
                newName: "ISIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "ClosingExchangeRateDate",
                table: "StockCalculations",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "StockCalculations",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OpeningExchangeRateDate",
                table: "StockCalculations",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ExchangeRateDate",
                table: "SoldCryptoCalculations",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ExchangeRateDate",
                table: "PurchasedCryptoCalculations",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ExchangeRateDate",
                table: "DividendCalculations",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "CfdCalculations",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExchangeRateDate",
                table: "CfdCalculations",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClosingExchangeRateDate",
                table: "StockCalculations");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "StockCalculations");

            migrationBuilder.DropColumn(
                name: "OpeningExchangeRateDate",
                table: "StockCalculations");

            migrationBuilder.DropColumn(
                name: "ExchangeRateDate",
                table: "SoldCryptoCalculations");

            migrationBuilder.DropColumn(
                name: "ExchangeRateDate",
                table: "PurchasedCryptoCalculations");

            migrationBuilder.DropColumn(
                name: "ExchangeRateDate",
                table: "DividendCalculations");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "CfdCalculations");

            migrationBuilder.DropColumn(
                name: "ExchangeRateDate",
                table: "CfdCalculations");

            migrationBuilder.RenameColumn(
                name: "ISIN",
                table: "ClosedPositions",
                newName: "Comments");
        }
    }
}
