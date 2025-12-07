using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DepozytOpon.Data.Migrations
{
    /// <inheritdoc />
    public partial class test1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Depozyty_Opony_OponaId",
                table: "Depozyty");

            migrationBuilder.DropIndex(
                name: "IX_Depozyty_OponaId",
                table: "Depozyty");

            migrationBuilder.AlterColumn<string>(
                name: "OponaId",
                table: "Depozyty",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "OponaId",
                table: "Depozyty",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Depozyty_OponaId",
                table: "Depozyty",
                column: "OponaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Depozyty_Opony_OponaId",
                table: "Depozyty",
                column: "OponaId",
                principalTable: "Opony",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
