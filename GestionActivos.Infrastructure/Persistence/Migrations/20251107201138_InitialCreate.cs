using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionActivos.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CAT_CATEGORIA",
                columns: table => new
                {
                    IdCategoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CAT_CATEGORIA", x => x.IdCategoria);
                });

            migrationBuilder.CreateTable(
                name: "CAT_PERMISO",
                columns: table => new
                {
                    IdPermiso = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CAT_PERMISO", x => x.IdPermiso);
                });

            migrationBuilder.CreateTable(
                name: "CAT_ROL",
                columns: table => new
                {
                    IdRol = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CAT_ROL", x => x.IdRol);
                });

            migrationBuilder.CreateTable(
                name: "CAT_USUARIO",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombres = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ApellidoPaterno = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ApellidoMaterno = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ClaveFortia = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Correo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Contrasena = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CAT_USUARIO", x => x.IdUsuario);
                });

            migrationBuilder.CreateTable(
                name: "CAT_ROL_PERMISO",
                columns: table => new
                {
                    IdRol = table.Column<int>(type: "int", nullable: false),
                    IdPermiso = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CAT_ROL_PERMISO", x => new { x.IdRol, x.IdPermiso });
                    table.ForeignKey(
                        name: "FK_CAT_ROL_PERMISO_CAT_PERMISO_IdPermiso",
                        column: x => x.IdPermiso,
                        principalTable: "CAT_PERMISO",
                        principalColumn: "IdPermiso",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CAT_ROL_PERMISO_CAT_ROL_IdRol",
                        column: x => x.IdRol,
                        principalTable: "CAT_ROL",
                        principalColumn: "IdRol",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CAT_CONFIG_AUDITORIA",
                columns: table => new
                {
                    IdConfig = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Periodo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdResponsable = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CAT_CONFIG_AUDITORIA", x => x.IdConfig);
                    table.ForeignKey(
                        name: "FK_CAT_CONFIG_AUDITORIA_CAT_USUARIO_IdResponsable",
                        column: x => x.IdResponsable,
                        principalTable: "CAT_USUARIO",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MOV_ACTIVO",
                columns: table => new
                {
                    IdActivo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Imagen = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Categoria = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ResponsableId = table.Column<int>(type: "int", nullable: false),
                    IdCategoria = table.Column<int>(type: "int", nullable: false),
                    Marca = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Modelo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Etiqueta = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NumeroSerie = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Donacion = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Factura = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ValorAdquisicion = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Estatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Activo"),
                    FechaAdquisicion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    PortaEtiqueta = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MOV_ACTIVO", x => x.IdActivo);
                    table.ForeignKey(
                        name: "FK_MOV_ACTIVO_CAT_CATEGORIA_IdCategoria",
                        column: x => x.IdCategoria,
                        principalTable: "CAT_CATEGORIA",
                        principalColumn: "IdCategoria",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MOV_ACTIVO_CAT_USUARIO_ResponsableId",
                        column: x => x.ResponsableId,
                        principalTable: "CAT_USUARIO",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MOV_AUDITORIA",
                columns: table => new
                {
                    IdAuditoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tipo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IdAuditor = table.Column<int>(type: "int", nullable: false),
                    IdUsuarioAuditado = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    Observaciones = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MOV_AUDITORIA", x => x.IdAuditoria);
                    table.ForeignKey(
                        name: "FK_MOV_AUDITORIA_CAT_USUARIO_IdAuditor",
                        column: x => x.IdAuditor,
                        principalTable: "CAT_USUARIO",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MOV_AUDITORIA_CAT_USUARIO_IdUsuarioAuditado",
                        column: x => x.IdUsuarioAuditado,
                        principalTable: "CAT_USUARIO",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MOV_SOLICITUD",
                columns: table => new
                {
                    IdSolicitud = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdEmisor = table.Column<int>(type: "int", nullable: false),
                    IdReceptor = table.Column<int>(type: "int", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Mensaje = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Pendiente")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MOV_SOLICITUD", x => x.IdSolicitud);
                    table.ForeignKey(
                        name: "FK_MOV_SOLICITUD_CAT_USUARIO_IdEmisor",
                        column: x => x.IdEmisor,
                        principalTable: "CAT_USUARIO",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MOV_SOLICITUD_CAT_USUARIO_IdReceptor",
                        column: x => x.IdReceptor,
                        principalTable: "CAT_USUARIO",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MOV_USUARIO_ROL",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    IdRol = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MOV_USUARIO_ROL", x => new { x.IdUsuario, x.IdRol });
                    table.ForeignKey(
                        name: "FK_MOV_USUARIO_ROL_CAT_ROL_IdRol",
                        column: x => x.IdRol,
                        principalTable: "CAT_ROL",
                        principalColumn: "IdRol",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MOV_USUARIO_ROL_CAT_USUARIO_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "CAT_USUARIO",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "H_REUBICACION",
                columns: table => new
                {
                    IdReubicacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdActivo = table.Column<int>(type: "int", nullable: false),
                    IdUsuarioAnterior = table.Column<int>(type: "int", nullable: false),
                    IdUsuarioNuevo = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    Motivo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_H_REUBICACION", x => x.IdReubicacion);
                    table.ForeignKey(
                        name: "FK_H_REUBICACION_CAT_USUARIO_IdUsuarioAnterior",
                        column: x => x.IdUsuarioAnterior,
                        principalTable: "CAT_USUARIO",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_H_REUBICACION_CAT_USUARIO_IdUsuarioNuevo",
                        column: x => x.IdUsuarioNuevo,
                        principalTable: "CAT_USUARIO",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_H_REUBICACION_MOV_ACTIVO_IdActivo",
                        column: x => x.IdActivo,
                        principalTable: "MOV_ACTIVO",
                        principalColumn: "IdActivo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MOV_DIAGNOSTICO",
                columns: table => new
                {
                    IdDiagnostico = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdActivo = table.Column<int>(type: "int", nullable: false),
                    IdTecnico = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    Pieza = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ValorAdquisicion = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TramiteGarantia = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    SugerirBaja = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Motivo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MOV_DIAGNOSTICO", x => x.IdDiagnostico);
                    table.ForeignKey(
                        name: "FK_MOV_DIAGNOSTICO_CAT_USUARIO_IdTecnico",
                        column: x => x.IdTecnico,
                        principalTable: "CAT_USUARIO",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MOV_DIAGNOSTICO_MOV_ACTIVO_IdActivo",
                        column: x => x.IdActivo,
                        principalTable: "MOV_ACTIVO",
                        principalColumn: "IdActivo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "H_DETALLE_AUDITORIA",
                columns: table => new
                {
                    IdDetalle = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdAuditoria = table.Column<int>(type: "int", nullable: false),
                    IdActivo = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Comentarios = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_H_DETALLE_AUDITORIA", x => x.IdDetalle);
                    table.ForeignKey(
                        name: "FK_H_DETALLE_AUDITORIA_MOV_ACTIVO_IdActivo",
                        column: x => x.IdActivo,
                        principalTable: "MOV_ACTIVO",
                        principalColumn: "IdActivo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_H_DETALLE_AUDITORIA_MOV_AUDITORIA_IdAuditoria",
                        column: x => x.IdAuditoria,
                        principalTable: "MOV_AUDITORIA",
                        principalColumn: "IdAuditoria",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CAT_CONFIG_AUDITORIA_IdResponsable",
                table: "CAT_CONFIG_AUDITORIA",
                column: "IdResponsable");

            migrationBuilder.CreateIndex(
                name: "IX_CAT_ROL_PERMISO_IdPermiso",
                table: "CAT_ROL_PERMISO",
                column: "IdPermiso");

            migrationBuilder.CreateIndex(
                name: "IX_CAT_USUARIO_ClaveFortia",
                table: "CAT_USUARIO",
                column: "ClaveFortia",
                unique: true,
                filter: "[ClaveFortia] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_H_DETALLE_AUDITORIA_IdActivo",
                table: "H_DETALLE_AUDITORIA",
                column: "IdActivo");

            migrationBuilder.CreateIndex(
                name: "IX_H_DETALLE_AUDITORIA_IdAuditoria",
                table: "H_DETALLE_AUDITORIA",
                column: "IdAuditoria");

            migrationBuilder.CreateIndex(
                name: "IX_H_REUBICACION_IdActivo",
                table: "H_REUBICACION",
                column: "IdActivo");

            migrationBuilder.CreateIndex(
                name: "IX_H_REUBICACION_IdUsuarioAnterior",
                table: "H_REUBICACION",
                column: "IdUsuarioAnterior");

            migrationBuilder.CreateIndex(
                name: "IX_H_REUBICACION_IdUsuarioNuevo",
                table: "H_REUBICACION",
                column: "IdUsuarioNuevo");

            migrationBuilder.CreateIndex(
                name: "IX_MOV_ACTIVO_Etiqueta",
                table: "MOV_ACTIVO",
                column: "Etiqueta",
                unique: true,
                filter: "[Etiqueta] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MOV_ACTIVO_IdCategoria",
                table: "MOV_ACTIVO",
                column: "IdCategoria");

            migrationBuilder.CreateIndex(
                name: "IX_MOV_ACTIVO_ResponsableId",
                table: "MOV_ACTIVO",
                column: "ResponsableId");

            migrationBuilder.CreateIndex(
                name: "IX_MOV_AUDITORIA_IdAuditor",
                table: "MOV_AUDITORIA",
                column: "IdAuditor");

            migrationBuilder.CreateIndex(
                name: "IX_MOV_AUDITORIA_IdUsuarioAuditado",
                table: "MOV_AUDITORIA",
                column: "IdUsuarioAuditado");

            migrationBuilder.CreateIndex(
                name: "IX_MOV_DIAGNOSTICO_IdActivo",
                table: "MOV_DIAGNOSTICO",
                column: "IdActivo");

            migrationBuilder.CreateIndex(
                name: "IX_MOV_DIAGNOSTICO_IdTecnico",
                table: "MOV_DIAGNOSTICO",
                column: "IdTecnico");

            migrationBuilder.CreateIndex(
                name: "IX_MOV_SOLICITUD_IdEmisor",
                table: "MOV_SOLICITUD",
                column: "IdEmisor");

            migrationBuilder.CreateIndex(
                name: "IX_MOV_SOLICITUD_IdReceptor",
                table: "MOV_SOLICITUD",
                column: "IdReceptor");

            migrationBuilder.CreateIndex(
                name: "IX_MOV_USUARIO_ROL_IdRol",
                table: "MOV_USUARIO_ROL",
                column: "IdRol");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CAT_CONFIG_AUDITORIA");

            migrationBuilder.DropTable(
                name: "CAT_ROL_PERMISO");

            migrationBuilder.DropTable(
                name: "H_DETALLE_AUDITORIA");

            migrationBuilder.DropTable(
                name: "H_REUBICACION");

            migrationBuilder.DropTable(
                name: "MOV_DIAGNOSTICO");

            migrationBuilder.DropTable(
                name: "MOV_SOLICITUD");

            migrationBuilder.DropTable(
                name: "MOV_USUARIO_ROL");

            migrationBuilder.DropTable(
                name: "CAT_PERMISO");

            migrationBuilder.DropTable(
                name: "MOV_AUDITORIA");

            migrationBuilder.DropTable(
                name: "MOV_ACTIVO");

            migrationBuilder.DropTable(
                name: "CAT_ROL");

            migrationBuilder.DropTable(
                name: "CAT_CATEGORIA");

            migrationBuilder.DropTable(
                name: "CAT_USUARIO");
        }
    }
}
