using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionActivos.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateActivoFacturasAndCuentaContable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Factura",
                table: "MOV_ACTIVO",
                newName: "CuentaContableEtiqueta");

            migrationBuilder.AddColumn<string>(
                name: "CuentaContable",
                table: "MOV_ACTIVO",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FacturaPDF",
                table: "MOV_ACTIVO",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FacturaXML",
                table: "MOV_ACTIVO",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CuentaContable",
                table: "MOV_ACTIVO");

            migrationBuilder.DropColumn(
                name: "FacturaPDF",
                table: "MOV_ACTIVO");

            migrationBuilder.DropColumn(
                name: "FacturaXML",
                table: "MOV_ACTIVO");

            migrationBuilder.RenameColumn(
                name: "CuentaContableEtiqueta",
                table: "MOV_ACTIVO",
                newName: "Factura");
        }
    }
}
