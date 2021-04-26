using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class USDValues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ClosingValue",
                table: "StockCalculations",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "OpeningValue",
                table: "StockCalculations",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Profit",
                table: "StockCalculations",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ClosingValue",
                table: "CryptoCalculations",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "OpeningValue",
                table: "CryptoCalculations",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Profit",
                table: "CryptoCalculations",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClosingValue",
                table: "StockCalculations");

            migrationBuilder.DropColumn(
                name: "OpeningValue",
                table: "StockCalculations");

            migrationBuilder.DropColumn(
                name: "Profit",
                table: "StockCalculations");

            migrationBuilder.DropColumn(
                name: "ClosingValue",
                table: "CryptoCalculations");

            migrationBuilder.DropColumn(
                name: "OpeningValue",
                table: "CryptoCalculations");

            migrationBuilder.DropColumn(
                name: "Profit",
                table: "CryptoCalculations");
        }
    }
}
