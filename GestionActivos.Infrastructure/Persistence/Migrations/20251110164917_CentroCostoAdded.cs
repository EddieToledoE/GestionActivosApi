using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionActivos.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CentroCostoAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdCentroCosto",
                table: "CAT_USUARIO",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CAT_CENTRO_COSTO",
                columns: table => new
                {
                    IdCentroCosto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ubicacion = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    RazonSocial = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Area = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CAT_CENTRO_COSTO", x => x.IdCentroCosto);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CAT_USUARIO_IdCentroCosto",
                table: "CAT_USUARIO",
                column: "IdCentroCosto");

            migrationBuilder.AddForeignKey(
                name: "FK_CAT_USUARIO_CAT_CENTRO_COSTO_IdCentroCosto",
                table: "CAT_USUARIO",
                column: "IdCentroCosto",
                principalTable: "CAT_CENTRO_COSTO",
                principalColumn: "IdCentroCosto",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CAT_USUARIO_CAT_CENTRO_COSTO_IdCentroCosto",
                table: "CAT_USUARIO");

            migrationBuilder.DropTable(
                name: "CAT_CENTRO_COSTO");

            migrationBuilder.DropIndex(
                name: "IX_CAT_USUARIO_IdCentroCosto",
                table: "CAT_USUARIO");

            migrationBuilder.DropColumn(
                name: "IdCentroCosto",
                table: "CAT_USUARIO");
        }
    }
}
