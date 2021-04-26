using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class CrpytoAndStockSecond : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CryptoCalculations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    PurchaseDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SellDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Units = table.Column<decimal>(type: "TEXT", nullable: false),
                    OpeningRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    ClosingRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    CurrencySymbol = table.Column<string>(type: "TEXT", nullable: true),
                    OpeningExchangeRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    ClosingExchangeRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    GainExchangedValue = table.Column<decimal>(type: "TEXT", nullable: false),
                    LossExchangedValue = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CryptoCalculations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StockCalculations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    PurchaseDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SellDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Units = table.Column<decimal>(type: "TEXT", nullable: false),
                    OpeningRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    ClosingRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    CurrencySymbol = table.Column<string>(type: "TEXT", nullable: true),
                    OpeningExchangeRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    ClosingExchangeRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    GainExchangedValue = table.Column<decimal>(type: "TEXT", nullable: false),
                    LossExchangedValue = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockCalculations", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CryptoCalculations");

            migrationBuilder.DropTable(
                name: "StockCalculations");
        }
    }
}
