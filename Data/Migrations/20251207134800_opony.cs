using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DepozytOpon.Data.Migrations
{
    /// <inheritdoc />
    public partial class opony : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Opony",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Typ = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Producent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rozmiar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bieżnik = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sezon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RokProdukcji = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KodTowaru = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Opony", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Opony");
        }
    }
}
