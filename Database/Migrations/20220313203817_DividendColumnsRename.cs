using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    public partial class DividendColumnsRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NetDividendReceivedExchanged",
                table: "DividendCalculations",
                newName: "DividendReceivedExchanged");

            migrationBuilder.RenameColumn(
                name: "NetDividendReceived",
                table: "DividendCalculations",
                newName: "DividendReceived");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DividendReceivedExchanged",
                table: "DividendCalculations",
                newName: "NetDividendReceivedExchanged");

            migrationBuilder.RenameColumn(
                name: "DividendReceived",
                table: "DividendCalculations",
                newName: "NetDividendReceived");
        }
    }
}
