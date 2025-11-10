using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionActivos.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ImagenActivoTypeChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Imagen",
                table: "MOV_ACTIVO");

            migrationBuilder.AddColumn<string>(
                name: "ImagenUrl",
                table: "MOV_ACTIVO",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagenUrl",
                table: "MOV_ACTIVO");

            migrationBuilder.AddColumn<byte[]>(
                name: "Imagen",
                table: "MOV_ACTIVO",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
