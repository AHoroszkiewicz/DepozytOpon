using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DepozytOpon.Migrations
{
    public partial class AddAuditColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Modyfikacja kolumn istniejących
            migrationBuilder.AlterColumn<string>(
                name: "RejestracjaPojazdu",
                table: "Depozyt",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "OponaId",
                table: "Depozyt",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "NumerTelefonu",
                table: "Depozyt",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "NumerBOX",
                table: "Depozyt",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Notatka",
                table: "Depozyt",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "MarkaPojazdu",
                table: "Depozyt",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ImieNazwisko",
                table: "Depozyt",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Ilosc",
                table: "Depozyt",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            // Dodanie nowych kolumn audytu
            migrationBuilder.AddColumn<DateTime>(
                name: "DataEdycji",
                table: "Depozyt",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EdytowanoPrzez",
                table: "Depozyt",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UtworzonoPrzez",
                table: "Depozyt",
                type: "nvarchar(max)",
                nullable: true);

            // Trigger aktualizujący DataEdycji i EdytowanoPrzez
            migrationBuilder.Sql(@"
CREATE TRIGGER trg_Depozyt_Update
ON Depozyt
AFTER UPDATE
AS
BEGIN
    UPDATE d
    SET DataEdycji = GETDATE(),
        EdytowanoPrzez = SYSTEM_USER
    FROM Depozyt d
    INNER JOIN inserted i ON d.Id = i.Id
END
");

            // Tabela historii depozytów
            migrationBuilder.Sql(@"
CREATE TABLE DepozytHistory (
    Id INT IDENTITY PRIMARY KEY,
    NumerBOX NVARCHAR(MAX),
    ImieNazwisko NVARCHAR(MAX),
    NumerTelefonu NVARCHAR(MAX),
    MarkaPojazdu NVARCHAR(MAX),
    RejestracjaPojazdu NVARCHAR(MAX),
    OponaId NVARCHAR(MAX),
    Ilosc INT,
    DataPrzyjecia DATETIME,
    Notatka NVARCHAR(MAX),
    UtworzonoPrzez NVARCHAR(MAX),
    DataEdycji DATETIME,
    EdytowanoPrzez NVARCHAR(MAX),
    DataUsuniecia DATETIME DEFAULT GETDATE()
);
");

            // Trigger kopiujący usunięte rekordy do historii
            migrationBuilder.Sql(@"
CREATE TRIGGER trg_Depozyt_Delete
ON Depozyt
AFTER DELETE
AS
BEGIN
    INSERT INTO DepozytHistory (NumerBOX, ImieNazwisko, NumerTelefonu, MarkaPojazdu, RejestracjaPojazdu, OponaId, Ilosc, DataPrzyjecia, Notatka, UtworzonoPrzez, DataEdycji, EdytowanoPrzez, DataUsuniecia)
    SELECT NumerBOX, ImieNazwisko, NumerTelefonu, MarkaPojazdu, RejestracjaPojazdu, OponaId, Ilosc, DataPrzyjecia, Notatka, UtworzonoPrzez, DataEdycji, EdytowanoPrzez, GETDATE()
    FROM deleted;
END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Usunięcie kolumn audytu
            migrationBuilder.DropColumn(name: "DataEdycji", table: "Depozyt");
            migrationBuilder.DropColumn(name: "EdytowanoPrzez", table: "Depozyt");
            migrationBuilder.DropColumn(name: "UtworzonoPrzez", table: "Depozyt");

            // Przywrócenie poprzednich ustawień kolumn
            migrationBuilder.AlterColumn<string>(
                name: "RejestracjaPojazdu",
                table: "Depozyt",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OponaId",
                table: "Depozyt",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NumerTelefonu",
                table: "Depozyt",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NumerBOX",
                table: "Depozyt",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Notatka",
                table: "Depozyt",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MarkaPojazdu",
                table: "Depozyt",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ImieNazwisko",
                table: "Depozyt",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Ilosc",
                table: "Depozyt",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            // Usunięcie triggerów i tabeli historii
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_Depozyt_Update");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_Depozyt_Delete");
            migrationBuilder.Sql("DROP TABLE IF EXISTS DepozytHistory");
        }
    }
}
