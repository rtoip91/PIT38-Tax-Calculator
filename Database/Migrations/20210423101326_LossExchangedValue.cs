using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class LossExchangedValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "LossExchangedValue",
                table: "CfdCalculations",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LossExchangedValue",
                table: "CfdCalculations");
        }
    }
}
