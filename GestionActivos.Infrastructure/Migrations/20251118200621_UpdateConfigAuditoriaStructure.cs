using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionActivos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateConfigAuditoriaStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaFin",
                table: "CAT_CONFIG_AUDITORIA");

            migrationBuilder.DropColumn(
                name: "FechaInicio",
                table: "CAT_CONFIG_AUDITORIA");

            migrationBuilder.AlterColumn<string>(
                name: "Periodo",
                table: "CAT_CONFIG_AUDITORIA",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Mensual",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "IdResponsable",
                table: "CAT_CONFIG_AUDITORIA",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<bool>(
                name: "Activa",
                table: "CAT_CONFIG_AUDITORIA",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<int>(
                name: "IdCentroCosto",
                table: "CAT_CONFIG_AUDITORIA",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CAT_CONFIG_AUDITORIA_IdCentroCosto",
                table: "CAT_CONFIG_AUDITORIA",
                column: "IdCentroCosto");

            migrationBuilder.AddForeignKey(
                name: "FK_CAT_CONFIG_AUDITORIA_CAT_CENTRO_COSTO_IdCentroCosto",
                table: "CAT_CONFIG_AUDITORIA",
                column: "IdCentroCosto",
                principalTable: "CAT_CENTRO_COSTO",
                principalColumn: "IdCentroCosto",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CAT_CONFIG_AUDITORIA_CAT_CENTRO_COSTO_IdCentroCosto",
                table: "CAT_CONFIG_AUDITORIA");

            migrationBuilder.DropIndex(
                name: "IX_CAT_CONFIG_AUDITORIA_IdCentroCosto",
                table: "CAT_CONFIG_AUDITORIA");

            migrationBuilder.DropColumn(
                name: "Activa",
                table: "CAT_CONFIG_AUDITORIA");

            migrationBuilder.DropColumn(
                name: "IdCentroCosto",
                table: "CAT_CONFIG_AUDITORIA");

            migrationBuilder.AlterColumn<string>(
                name: "Periodo",
                table: "CAT_CONFIG_AUDITORIA",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldDefaultValue: "Mensual");

            migrationBuilder.AlterColumn<Guid>(
                name: "IdResponsable",
                table: "CAT_CONFIG_AUDITORIA",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaFin",
                table: "CAT_CONFIG_AUDITORIA",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaInicio",
                table: "CAT_CONFIG_AUDITORIA",
                type: "datetime2",
                nullable: true);
        }
    }
}
