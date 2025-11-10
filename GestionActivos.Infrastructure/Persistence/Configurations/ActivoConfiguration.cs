using GestionActivos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionActivos.Infrastructure.Persistence.Configurations
{
    public class ActivoConfiguration : IEntityTypeConfiguration<Activo>
    {
        public void Configure(EntityTypeBuilder<Activo> builder)
        {
            builder.ToTable("MOV_ACTIVO");

            builder.HasKey(a => a.IdActivo);

            builder
                .Property(a => a.ImagenUrl)
                .HasMaxLength(400)
                .HasColumnName("ImagenUrl")
                .IsRequired(false);

            builder.Property(a => a.Marca).HasMaxLength(100);

            builder.Property(a => a.Modelo).HasMaxLength(100);

            builder.Property(a => a.Descripcion).HasMaxLength(200);

            builder.Property(a => a.Etiqueta).HasMaxLength(50);

            builder.HasIndex(a => a.Etiqueta).IsUnique();

            builder.Property(a => a.NumeroSerie).HasMaxLength(100);

            builder.Property(a => a.Factura).HasMaxLength(100);

            builder.Property(a => a.ValorAdquisicion).HasPrecision(12, 2);

            builder.Property(a => a.Estatus).HasMaxLength(50).HasDefaultValue("Activo");

            builder.Property(a => a.Donacion).HasDefaultValue(false);

            builder.Property(a => a.PortaEtiqueta).HasDefaultValue(false);

            builder.Property(a => a.FechaAlta).HasDefaultValueSql("GETDATE()");

            // 🔗 Relaciones
            builder
                .HasOne(a => a.Responsable)
                .WithMany(u => u.ActivosResponsables)
                .HasForeignKey(a => a.ResponsableId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(a => a.CategoriaNavigation)
                .WithMany(c => c.Activos)
                .HasForeignKey(a => a.IdCategoria)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
        }
    }
}
