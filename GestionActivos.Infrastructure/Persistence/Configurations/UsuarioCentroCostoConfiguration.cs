using GestionActivos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionActivos.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración Fluent API para la entidad UsuarioCentroCosto.
    /// Define la relación muchos-a-muchos entre Usuario y CentroCosto.
    /// </summary>
    public class UsuarioCentroCostoConfiguration : IEntityTypeConfiguration<UsuarioCentroCosto>
    {
        public void Configure(EntityTypeBuilder<UsuarioCentroCosto> builder)
        {
            // Nombre de la tabla
            builder.ToTable("MOV_USUARIO_CENTRO_COSTO");

            // Clave primaria compuesta
            builder.HasKey(uc => new { uc.IdUsuario, uc.IdCentroCosto });

            // Propiedades
            builder.Property(uc => uc.FechaInicio)
                .HasColumnType("datetime2")
                .IsRequired(false);

            builder.Property(uc => uc.FechaFin)
                .HasColumnType("datetime2")
                .IsRequired(false);

            builder.Property(uc => uc.Activo)
                .IsRequired()
                .HasDefaultValue(true);

            // Relación con Usuario
            builder.HasOne(uc => uc.Usuario)
                .WithMany(u => u.UsuarioCentrosCosto)
                .HasForeignKey(uc => uc.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación con CentroCosto
            builder.HasOne(uc => uc.CentroCosto)
                .WithMany(cc => cc.UsuariosAsignados)
                .HasForeignKey(uc => uc.IdCentroCosto)
                .OnDelete(DeleteBehavior.Restrict);

            // Índices para mejorar consultas
            builder.HasIndex(uc => uc.IdUsuario);
            builder.HasIndex(uc => uc.IdCentroCosto);
            builder.HasIndex(uc => uc.Activo);
        }
    }
}
