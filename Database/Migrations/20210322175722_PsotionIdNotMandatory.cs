using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class PsotionIdNotMandatory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionReports_ClosedPositions_PositionId",
                table: "TransactionReports");

            migrationBuilder.AlterColumn<int>(
                name: "PositionId",
                table: "TransactionReports",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionReports_ClosedPositions_PositionId",
                table: "TransactionReports",
                column: "PositionId",
                principalTable: "ClosedPositions",
                principalColumn: "PositionId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionReports_ClosedPositions_PositionId",
                table: "TransactionReports");

            migrationBuilder.AlterColumn<int>(
                name: "PositionId",
                table: "TransactionReports",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionReports_ClosedPositions_PositionId",
                table: "TransactionReports",
                column: "PositionId",
                principalTable: "ClosedPositions",
                principalColumn: "PositionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
