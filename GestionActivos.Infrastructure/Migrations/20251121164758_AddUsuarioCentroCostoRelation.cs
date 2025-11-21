using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionActivos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUsuarioCentroCostoRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MOV_USUARIO_CENTRO_COSTO",
                columns: table => new
                {
                    IdUsuario = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdCentroCosto = table.Column<int>(type: "int", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MOV_USUARIO_CENTRO_COSTO", x => new { x.IdUsuario, x.IdCentroCosto });
                    table.ForeignKey(
                        name: "FK_MOV_USUARIO_CENTRO_COSTO_CAT_CENTRO_COSTO_IdCentroCosto",
                        column: x => x.IdCentroCosto,
                        principalTable: "CAT_CENTRO_COSTO",
                        principalColumn: "IdCentroCosto",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MOV_USUARIO_CENTRO_COSTO_CAT_USUARIO_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "CAT_USUARIO",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MOV_USUARIO_CENTRO_COSTO_Activo",
                table: "MOV_USUARIO_CENTRO_COSTO",
                column: "Activo");

            migrationBuilder.CreateIndex(
                name: "IX_MOV_USUARIO_CENTRO_COSTO_IdCentroCosto",
                table: "MOV_USUARIO_CENTRO_COSTO",
                column: "IdCentroCosto");

            migrationBuilder.CreateIndex(
                name: "IX_MOV_USUARIO_CENTRO_COSTO_IdUsuario",
                table: "MOV_USUARIO_CENTRO_COSTO",
                column: "IdUsuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MOV_USUARIO_CENTRO_COSTO");
        }
    }
}
