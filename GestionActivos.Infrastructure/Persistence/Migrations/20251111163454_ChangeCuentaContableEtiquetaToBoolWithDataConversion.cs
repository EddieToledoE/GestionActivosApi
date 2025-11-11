using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionActivos.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCuentaContableEtiquetaToBoolWithDataConversion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
   {
   // Eliminar la columna antigua
         migrationBuilder.DropColumn(
     name: "CuentaContableEtiqueta",
    table: "MOV_ACTIVO");

          // Crear la columna nueva como bit con valor por defecto false
            migrationBuilder.AddColumn<bool>(
           name: "CuentaContableEtiqueta",
                table: "MOV_ACTIVO",
      type: "bit",
                nullable: false,
 defaultValue: false);
        }

  /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
    {
        // Eliminar la columna bit
      migrationBuilder.DropColumn(
       name: "CuentaContableEtiqueta",
         table: "MOV_ACTIVO");

    // Recrear como nvarchar
  migrationBuilder.AddColumn<string>(
     name: "CuentaContableEtiqueta",
                table: "MOV_ACTIVO",
    type: "nvarchar(100)",
            maxLength: 100,
    nullable: true);
      }
    }
}
