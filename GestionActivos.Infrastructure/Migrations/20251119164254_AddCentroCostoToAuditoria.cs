using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionActivos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCentroCostoToAuditoria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdCentroCosto",
                table: "MOV_AUDITORIA",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MOV_AUDITORIA_IdCentroCosto",
                table: "MOV_AUDITORIA",
                column: "IdCentroCosto");

            migrationBuilder.AddForeignKey(
                name: "FK_MOV_AUDITORIA_CAT_CENTRO_COSTO_IdCentroCosto",
                table: "MOV_AUDITORIA",
                column: "IdCentroCosto",
                principalTable: "CAT_CENTRO_COSTO",
                principalColumn: "IdCentroCosto",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MOV_AUDITORIA_CAT_CENTRO_COSTO_IdCentroCosto",
                table: "MOV_AUDITORIA");

            migrationBuilder.DropIndex(
                name: "IX_MOV_AUDITORIA_IdCentroCosto",
                table: "MOV_AUDITORIA");

            migrationBuilder.DropColumn(
                name: "IdCentroCosto",
                table: "MOV_AUDITORIA");
        }
    }
}
