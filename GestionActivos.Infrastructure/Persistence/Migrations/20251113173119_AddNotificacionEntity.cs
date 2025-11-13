using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionActivos.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddNotificacionEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MOV_NOTIFICACION",
                columns: table => new
                {
                    IdNotificacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuarioDestino = table.Column<int>(type: "int", nullable: false),
                    Origen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Tipo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Mensaje = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    Leida = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MOV_NOTIFICACION", x => x.IdNotificacion);
                    table.ForeignKey(
                        name: "FK_MOV_NOTIFICACION_CAT_USUARIO_IdUsuarioDestino",
                        column: x => x.IdUsuarioDestino,
                        principalTable: "CAT_USUARIO",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MOV_NOTIFICACION_IdUsuarioDestino",
                table: "MOV_NOTIFICACION",
                column: "IdUsuarioDestino");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MOV_NOTIFICACION");
        }
    }
}
