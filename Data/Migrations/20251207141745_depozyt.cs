using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DepozytOpon.Data.Migrations
{
    /// <inheritdoc />
    public partial class depozyt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Depozyty",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumerBOX = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImieNazwisko = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumerTelefonu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MarkaPojazdu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RejestracjaPojazdu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OponaId = table.Column<int>(type: "int", nullable: false),
                    Ilosc = table.Column<int>(type: "int", nullable: false),
                    DataPrzyjecia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notatka = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Depozyty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Depozyty_Opony_OponaId",
                        column: x => x.OponaId,
                        principalTable: "Opony",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Depozyty_OponaId",
                table: "Depozyty",
                column: "OponaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Depozyty");
        }
    }
}
