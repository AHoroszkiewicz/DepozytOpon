using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DepozytOpon.Data.Migrations
{
    /// <inheritdoc />
    public partial class zmiananazwybieżnika : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Bieżnik",
                table: "Opony",
                newName: "Bieznik");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Bieznik",
                table: "Opony",
                newName: "Bieżnik");
        }
    }
}
