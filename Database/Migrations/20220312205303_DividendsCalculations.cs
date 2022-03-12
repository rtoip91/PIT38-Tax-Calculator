using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    public partial class DividendsCalculations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DividendCalculations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PositionId = table.Column<int>(type: "INTEGER", nullable: false),
                    InstrumentName = table.Column<string>(type: "TEXT", nullable: true),
                    DateOfPayment = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Currency = table.Column<string>(type: "TEXT", nullable: true),
                    ExchangeRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    NetDividendReceived = table.Column<decimal>(type: "TEXT", nullable: false),
                    NetDividendReceivedExchanged = table.Column<decimal>(type: "TEXT", nullable: false),
                    WithholdingTaxRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    WithholdingTaxPaid = table.Column<decimal>(type: "TEXT", nullable: false),
                    WithholdingTaxRemain = table.Column<decimal>(type: "TEXT", nullable: false),
                    Country = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DividendCalculations", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DividendCalculations");
        }
    }
}
