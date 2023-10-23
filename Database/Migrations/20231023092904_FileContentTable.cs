using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class FileContentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_File_OperationGuid_Status",
                table: "File");

            migrationBuilder.DropColumn(
                name: "CalculationResultFileContent",
                table: "File");

            migrationBuilder.DropColumn(
                name: "InputFileContent",
                table: "File");

            migrationBuilder.AddColumn<int>(
                name: "CalculationResultFileContentId",
                table: "File",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InputFileContentId",
                table: "File",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FileContent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FileType = table.Column<int>(type: "integer", nullable: false),
                    FileContent = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileContent", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_File_CalculationResultFileContentId",
                table: "File",
                column: "CalculationResultFileContentId");

            migrationBuilder.CreateIndex(
                name: "IX_File_InputFileContentId",
                table: "File",
                column: "InputFileContentId");

            migrationBuilder.AddForeignKey(
                name: "FK_File_FileContent_CalculationResultFileContentId",
                table: "File",
                column: "CalculationResultFileContentId",
                principalTable: "FileContent",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_File_FileContent_InputFileContentId",
                table: "File",
                column: "InputFileContentId",
                principalTable: "FileContent",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_File_FileContent_CalculationResultFileContentId",
                table: "File");

            migrationBuilder.DropForeignKey(
                name: "FK_File_FileContent_InputFileContentId",
                table: "File");

            migrationBuilder.DropTable(
                name: "FileContent");

            migrationBuilder.DropIndex(
                name: "IX_File_CalculationResultFileContentId",
                table: "File");

            migrationBuilder.DropIndex(
                name: "IX_File_InputFileContentId",
                table: "File");

            migrationBuilder.DropColumn(
                name: "CalculationResultFileContentId",
                table: "File");

            migrationBuilder.DropColumn(
                name: "InputFileContentId",
                table: "File");

            migrationBuilder.AddColumn<byte[]>(
                name: "CalculationResultFileContent",
                table: "File",
                type: "bytea",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "InputFileContent",
                table: "File",
                type: "bytea",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_File_OperationGuid_Status",
                table: "File",
                columns: new[] { "OperationGuid", "Status" });
        }
    }
}
