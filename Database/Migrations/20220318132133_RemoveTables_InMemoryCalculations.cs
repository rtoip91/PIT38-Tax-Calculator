using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    public partial class RemoveTables_InMemoryCalculations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CfdCalculations");

            migrationBuilder.DropTable(
                name: "DividendCalculations");

            migrationBuilder.DropTable(
                name: "Dividends");

            migrationBuilder.DropTable(
                name: "PurchasedCryptoCalculations");

            migrationBuilder.DropTable(
                name: "SoldCryptoCalculations");

            migrationBuilder.DropTable(
                name: "StockCalculations");

            migrationBuilder.DropTable(
                name: "TransactionReports");

            migrationBuilder.DropTable(
                name: "ClosedPositions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CfdCalculations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClosingRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    Country = table.Column<string>(type: "TEXT", nullable: true),
                    CurrencySymbol = table.Column<string>(type: "TEXT", nullable: true),
                    ExchangeRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    ExchangeRateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GainExchangedValue = table.Column<decimal>(type: "TEXT", nullable: false),
                    GainValue = table.Column<decimal>(type: "TEXT", nullable: false),
                    Leverage = table.Column<int>(type: "INTEGER", nullable: false),
                    LossExchangedValue = table.Column<decimal>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    OpeningRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    PositionId = table.Column<long>(type: "INTEGER", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SellDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TransactionType = table.Column<int>(type: "INTEGER", nullable: false),
                    Units = table.Column<decimal>(type: "TEXT", nullable: false)
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
                    Amount = table.Column<decimal>(type: "TEXT", nullable: true),
                    ClosingDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ClosingRate = table.Column<decimal>(type: "TEXT", nullable: true),
                    CopiedInvestor = table.Column<string>(type: "TEXT", nullable: true),
                    FeesAndDividends = table.Column<decimal>(type: "TEXT", nullable: true),
                    ISIN = table.Column<string>(type: "TEXT", nullable: true),
                    IsReal = table.Column<string>(type: "TEXT", nullable: true),
                    Leverage = table.Column<int>(type: "INTEGER", nullable: false),
                    OpeningDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    OpeningRate = table.Column<decimal>(type: "TEXT", nullable: true),
                    Operation = table.Column<string>(type: "TEXT", nullable: true),
                    Profit = table.Column<decimal>(type: "TEXT", nullable: true),
                    Spread = table.Column<decimal>(type: "TEXT", nullable: true),
                    StopLossRate = table.Column<decimal>(type: "TEXT", nullable: true),
                    TakeProfitRate = table.Column<decimal>(type: "TEXT", nullable: true),
                    TransactionType = table.Column<int>(type: "INTEGER", nullable: false),
                    Units = table.Column<decimal>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClosedPositions", x => x.PositionId);
                });

            migrationBuilder.CreateTable(
                name: "DividendCalculations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Country = table.Column<string>(type: "TEXT", nullable: true),
                    Currency = table.Column<string>(type: "TEXT", nullable: true),
                    DateOfPayment = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DividendReceived = table.Column<decimal>(type: "TEXT", nullable: false),
                    DividendReceivedExchanged = table.Column<decimal>(type: "TEXT", nullable: false),
                    ExchangeRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    ExchangeRateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    InstrumentName = table.Column<string>(type: "TEXT", nullable: true),
                    PositionId = table.Column<int>(type: "INTEGER", nullable: false),
                    WithholdingTaxPaid = table.Column<decimal>(type: "TEXT", nullable: false),
                    WithholdingTaxRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    WithholdingTaxRemain = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DividendCalculations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Dividends",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DateOfPayment = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ISIN = table.Column<string>(type: "TEXT", nullable: true),
                    InstrumentName = table.Column<string>(type: "TEXT", nullable: true),
                    NetDividendReceived = table.Column<decimal>(type: "TEXT", nullable: false),
                    PositionId = table.Column<int>(type: "INTEGER", nullable: false),
                    PositionType = table.Column<string>(type: "TEXT", nullable: true),
                    WithholdingTaxAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    WithholdingTaxRate = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dividends", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PurchasedCryptoCalculations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CurrencySymbol = table.Column<string>(type: "TEXT", nullable: true),
                    ExchangeRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    ExchangeRateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    PositionId = table.Column<long>(type: "INTEGER", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TotalExchangedValue = table.Column<decimal>(type: "TEXT", nullable: false),
                    TotalValue = table.Column<decimal>(type: "TEXT", nullable: false),
                    Units = table.Column<decimal>(type: "TEXT", nullable: false),
                    ValuePerUnit = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchasedCryptoCalculations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SoldCryptoCalculations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CurrencySymbol = table.Column<string>(type: "TEXT", nullable: true),
                    ExchangeRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    ExchangeRateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    PositionId = table.Column<long>(type: "INTEGER", nullable: false),
                    SellDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TotalExchangedValue = table.Column<decimal>(type: "TEXT", nullable: false),
                    TotalValue = table.Column<decimal>(type: "TEXT", nullable: false),
                    Units = table.Column<decimal>(type: "TEXT", nullable: false),
                    ValuePerUnit = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoldCryptoCalculations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StockCalculations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClosingExchangeRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    ClosingExchangeRateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ClosingExchangedValue = table.Column<decimal>(type: "TEXT", nullable: false),
                    ClosingUnitValue = table.Column<decimal>(type: "TEXT", nullable: false),
                    ClosingValue = table.Column<decimal>(type: "TEXT", nullable: false),
                    Country = table.Column<string>(type: "TEXT", nullable: true),
                    CurrencySymbol = table.Column<string>(type: "TEXT", nullable: true),
                    ExchangedProfit = table.Column<decimal>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    OpeningExchangeRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    OpeningExchangeRateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    OpeningExchangedValue = table.Column<decimal>(type: "TEXT", nullable: false),
                    OpeningUnitValue = table.Column<decimal>(type: "TEXT", nullable: false),
                    OpeningValue = table.Column<decimal>(type: "TEXT", nullable: false),
                    PositionId = table.Column<long>(type: "INTEGER", nullable: false),
                    Profit = table.Column<decimal>(type: "TEXT", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SellDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Units = table.Column<decimal>(type: "TEXT", nullable: false)
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
                    PositionId = table.Column<int>(type: "INTEGER", nullable: true),
                    AccountBalance = table.Column<decimal>(type: "TEXT", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Details = table.Column<string>(type: "TEXT", nullable: true),
                    NWA = table.Column<int>(type: "INTEGER", nullable: false),
                    RealizedEquity = table.Column<decimal>(type: "TEXT", nullable: false),
                    RealizedEquityChange = table.Column<decimal>(type: "TEXT", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: true)
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
    }
}
