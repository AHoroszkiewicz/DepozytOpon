using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DepozytOpon.Migrations
{
    /// <inheritdoc />
    public partial class AddingRequiredOpona : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "KodTowaru",
                table: "Opony",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Opony_KodTowaru",
                table: "Opony",
                column: "KodTowaru",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Opony_KodTowaru",
                table: "Opony");

            migrationBuilder.AlterColumn<string>(
                name: "KodTowaru",
                table: "Opony",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
