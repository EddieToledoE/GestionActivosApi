using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionActivos.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCuentaContableEtiquetaAsBit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
      {
   // Esta columna ya fue agregada manualmente via SQL
            // Solo registramos el cambio en el historial de migraciones
        }

        /// <inheritdoc />
  protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
   name: "CuentaContableEtiqueta",
       table: "MOV_ACTIVO");
        }
    }
}
