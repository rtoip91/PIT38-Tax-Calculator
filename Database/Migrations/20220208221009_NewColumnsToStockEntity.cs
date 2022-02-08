using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    public partial class NewColumnsToStockEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ClosingUnitValue",
                table: "StockCalculations",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "OpeningUnitValue",
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClosingUnitValue",
                table: "StockCalculations");

            migrationBuilder.DropColumn(
                name: "OpeningUnitValue",
                table: "StockCalculations");

            migrationBuilder.DropColumn(
                name: "Units",
                table: "StockCalculations");
        }
    }
}
