using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionActivos.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddIdActivoToSolicitud : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdActivo",
                table: "MOV_SOLICITUD",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<bool>(
                name: "CuentaContableEtiqueta",
                table: "MOV_ACTIVO",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MOV_SOLICITUD_IdActivo",
                table: "MOV_SOLICITUD",
                column: "IdActivo");

            migrationBuilder.AddForeignKey(
                name: "FK_MOV_SOLICITUD_MOV_ACTIVO_IdActivo",
                table: "MOV_SOLICITUD",
                column: "IdActivo",
                principalTable: "MOV_ACTIVO",
                principalColumn: "IdActivo",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MOV_SOLICITUD_MOV_ACTIVO_IdActivo",
                table: "MOV_SOLICITUD");

            migrationBuilder.DropIndex(
                name: "IX_MOV_SOLICITUD_IdActivo",
                table: "MOV_SOLICITUD");

            migrationBuilder.DropColumn(
                name: "IdActivo",
                table: "MOV_SOLICITUD");

            migrationBuilder.AlterColumn<string>(
                name: "CuentaContableEtiqueta",
                table: "MOV_ACTIVO",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);
        }
    }
}
