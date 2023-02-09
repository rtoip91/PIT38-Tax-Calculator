using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class CrpytocurrenciesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "Cryptocurrency",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cryptocurrency", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeRates_Code_Date",
                table: "ExchangeRates",
                columns: new[] { "Code", "Date" });

            migrationBuilder.CreateIndex(
                name: "IX_Cryptocurrency_Code",
                table: "Cryptocurrency",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cryptocurrency");

            migrationBuilder.DropIndex(
                name: "IX_ExchangeRates_Code_Date",
                table: "ExchangeRates");

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
                columns: new[] { "Code", "Date" },
                unique: true);
        }
    }
}
