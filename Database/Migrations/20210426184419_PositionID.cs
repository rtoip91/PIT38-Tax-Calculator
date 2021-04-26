using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class PositionID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PositionId",
                table: "StockCalculations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "PositionId",
                table: "CryptoCalculations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "PositionId",
                table: "CfdCalculations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PositionId",
                table: "StockCalculations");

            migrationBuilder.DropColumn(
                name: "PositionId",
                table: "CryptoCalculations");

            migrationBuilder.DropColumn(
                name: "PositionId",
                table: "CfdCalculations");
        }
    }
}
