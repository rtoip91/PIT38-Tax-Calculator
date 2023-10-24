using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class SeparateTablesForInputAndOutputFiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_File_FileContent_CalculationResultFileContentId",
                table: "File");

            migrationBuilder.DropForeignKey(
                name: "FK_File_FileContent_InputFileContentId",
                table: "File");

            migrationBuilder.DropTable(
                name: "FileContent");

            migrationBuilder.CreateTable(
                name: "InputFileContent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FileContent = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InputFileContent", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResultFileContent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FileContent = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultFileContent", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_File_InputFileContent_InputFileContentId",
                table: "File",
                column: "InputFileContentId",
                principalTable: "InputFileContent",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_File_ResultFileContent_CalculationResultFileContentId",
                table: "File",
                column: "CalculationResultFileContentId",
                principalTable: "ResultFileContent",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_File_InputFileContent_InputFileContentId",
                table: "File");

            migrationBuilder.DropForeignKey(
                name: "FK_File_ResultFileContent_CalculationResultFileContentId",
                table: "File");

            migrationBuilder.DropTable(
                name: "InputFileContent");

            migrationBuilder.DropTable(
                name: "ResultFileContent");

            migrationBuilder.CreateTable(
                name: "FileContent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FileContent = table.Column<byte[]>(type: "bytea", nullable: true),
                    FileType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileContent", x => x.Id);
                });

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
    }
}
