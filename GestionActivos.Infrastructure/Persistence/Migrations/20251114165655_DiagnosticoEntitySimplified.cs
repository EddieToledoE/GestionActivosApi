using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionActivos.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DiagnosticoEntitySimplified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Motivo",
                table: "MOV_DIAGNOSTICO");

            migrationBuilder.DropColumn(
                name: "Pieza",
                table: "MOV_DIAGNOSTICO");

            migrationBuilder.DropColumn(
                name: "SugerirBaja",
                table: "MOV_DIAGNOSTICO");

            migrationBuilder.DropColumn(
                name: "TramiteGarantia",
                table: "MOV_DIAGNOSTICO");

            migrationBuilder.DropColumn(
                name: "ValorAdquisicion",
                table: "MOV_DIAGNOSTICO");

            migrationBuilder.AddColumn<string>(
                name: "Tipo",
                table: "MOV_DIAGNOSTICO",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "MOV_DIAGNOSTICO");

            migrationBuilder.AddColumn<string>(
                name: "Motivo",
                table: "MOV_DIAGNOSTICO",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pieza",
                table: "MOV_DIAGNOSTICO",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SugerirBaja",
                table: "MOV_DIAGNOSTICO",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TramiteGarantia",
                table: "MOV_DIAGNOSTICO",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorAdquisicion",
                table: "MOV_DIAGNOSTICO",
                type: "decimal(12,2)",
                precision: 12,
                scale: 2,
                nullable: true);
        }
    }
}
