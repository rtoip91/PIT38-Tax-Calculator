using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class UnnecessaryColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClosingRate",
                table: "StockCalculations");

            migrationBuilder.DropColumn(
                name: "OpeningRate",
                table: "StockCalculations");

            migrationBuilder.DropColumn(
                name: "Units",
                table: "StockCalculations");

            migrationBuilder.DropColumn(
                name: "ClosingRate",
                table: "CryptoCalculations");

            migrationBuilder.DropColumn(
                name: "OpeningRate",
                table: "CryptoCalculations");

            migrationBuilder.DropColumn(
                name: "Units",
                table: "CryptoCalculations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ClosingRate",
                table: "StockCalculations",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "OpeningRate",
                table: "StockCalculations",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Units",
                table: "StockCalculations",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ClosingRate",
                table: "CryptoCalculations",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "OpeningRate",
                table: "CryptoCalculations",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Units",
                table: "CryptoCalculations",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
