using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_File_OperationGuid",
                table: "File",
                column: "OperationGuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_File_OperationGuid_Status",
                table: "File",
                columns: new[] { "OperationGuid", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeRates_Code_Date",
                table: "ExchangeRates",
                columns: new[] { "Code", "Date" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_File_OperationGuid",
                table: "File");

            migrationBuilder.DropIndex(
                name: "IX_File_OperationGuid_Status",
                table: "File");

            migrationBuilder.DropIndex(
                name: "IX_ExchangeRates_Code_Date",
                table: "ExchangeRates");
        }
    }
}
