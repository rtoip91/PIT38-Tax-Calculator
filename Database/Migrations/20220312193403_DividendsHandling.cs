using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    public partial class DividendsHandling : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Leverage",
                table: "CfdCalculations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Dividends",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PositionId = table.Column<int>(type: "INTEGER", nullable: false),
                    DateOfPayment = table.Column<DateTime>(type: "TEXT", nullable: false),
                    InstrumentName = table.Column<string>(type: "TEXT", nullable: true),
                    NetDividendReceived = table.Column<decimal>(type: "TEXT", nullable: false),
                    WithholdingTaxRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    WithholdingTaxAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    PositionType = table.Column<string>(type: "TEXT", nullable: true),
                    ISIN = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dividends", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dividends");

            migrationBuilder.DropColumn(
                name: "Leverage",
                table: "CfdCalculations");
        }
    }
}
