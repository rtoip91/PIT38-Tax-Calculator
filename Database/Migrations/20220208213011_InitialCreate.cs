using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CfdCalculations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    PositionId = table.Column<long>(type: "INTEGER", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SellDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Units = table.Column<decimal>(type: "TEXT", nullable: false),
                    OpeningRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    ClosingRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    GainValue = table.Column<decimal>(type: "TEXT", nullable: false),
                    CurrencySymbol = table.Column<string>(type: "TEXT", nullable: true),
                    ExchangeRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    GainExchangedValue = table.Column<decimal>(type: "TEXT", nullable: false),
                    LossExchangedValue = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CfdCalculations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClosedPositions",
                columns: table => new
                {
                    PositionId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Operation = table.Column<string>(type: "TEXT", nullable: true),
                    CopiedInvestor = table.Column<string>(type: "TEXT", nullable: true),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: true),
                    Units = table.Column<decimal>(type: "TEXT", nullable: true),
                    OpeningRate = table.Column<decimal>(type: "TEXT", nullable: true),
                    ClosingRate = table.Column<decimal>(type: "TEXT", nullable: true),
                    Spread = table.Column<decimal>(type: "TEXT", nullable: true),
                    Profit = table.Column<decimal>(type: "TEXT", nullable: true),
                    OpeningDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ClosingDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TakeProfitRate = table.Column<decimal>(type: "TEXT", nullable: true),
                    StopLossRate = table.Column<decimal>(type: "TEXT", nullable: true),
                    FeesAndDividends = table.Column<decimal>(type: "TEXT", nullable: true),
                    IsReal = table.Column<string>(type: "TEXT", nullable: true),
                    Leverage = table.Column<int>(type: "INTEGER", nullable: false),
                    Comments = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClosedPositions", x => x.PositionId);
                });

            migrationBuilder.CreateTable(
                name: "CryptoCalculations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PositionId = table.Column<long>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    PurchaseDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SellDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CurrencySymbol = table.Column<string>(type: "TEXT", nullable: true),
                    Profit = table.Column<decimal>(type: "TEXT", nullable: false),
                    OpeningValue = table.Column<decimal>(type: "TEXT", nullable: false),
                    ClosingValue = table.Column<decimal>(type: "TEXT", nullable: false),
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
                name: "ExchangeRates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Currency = table.Column<string>(type: "TEXT", nullable: true),
                    Code = table.Column<string>(type: "TEXT", nullable: true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Rate = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangeRates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StockCalculations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PositionId = table.Column<long>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    PurchaseDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SellDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CurrencySymbol = table.Column<string>(type: "TEXT", nullable: true),
                    OpeningValue = table.Column<decimal>(type: "TEXT", nullable: false),
                    ClosingValue = table.Column<decimal>(type: "TEXT", nullable: false),
                    Profit = table.Column<decimal>(type: "TEXT", nullable: false),
                    ExchangedProfit = table.Column<decimal>(type: "TEXT", nullable: false),
                    OpeningExchangeRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    ClosingExchangeRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    ClosingExchangedValue = table.Column<decimal>(type: "TEXT", nullable: false),
                    OpeningExchangedValue = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockCalculations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AccountBalance = table.Column<decimal>(type: "TEXT", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: true),
                    Details = table.Column<string>(type: "TEXT", nullable: true),
                    PositionId = table.Column<int>(type: "INTEGER", nullable: true),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false),
                    RealizedEquityChange = table.Column<decimal>(type: "TEXT", nullable: false),
                    RealizedEquity = table.Column<decimal>(type: "TEXT", nullable: false),
                    NWA = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionReports_ClosedPositions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "ClosedPositions",
                        principalColumn: "PositionId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransactionReports_PositionId",
                table: "TransactionReports",
                column: "PositionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CfdCalculations");

            migrationBuilder.DropTable(
                name: "CryptoCalculations");

            migrationBuilder.DropTable(
                name: "ExchangeRates");

            migrationBuilder.DropTable(
                name: "StockCalculations");

            migrationBuilder.DropTable(
                name: "TransactionReports");

            migrationBuilder.DropTable(
                name: "ClosedPositions");
        }
    }
}
