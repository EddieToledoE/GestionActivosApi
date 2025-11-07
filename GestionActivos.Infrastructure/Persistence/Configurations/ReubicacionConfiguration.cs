using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GestionActivos.Domain.Entities;

namespace GestionActivos.Infrastructure.Persistence.Configurations
{
    public class ReubicacionConfiguration : IEntityTypeConfiguration<Reubicacion>
    {
        public void Configure(EntityTypeBuilder<Reubicacion> builder)
        {
            builder.ToTable("H_REUBICACION");
            builder.HasKey(r => r.IdReubicacion);

            builder.Property(r => r.Motivo)
                   .HasMaxLength(200);

            builder.Property(r => r.Fecha)
                   .HasDefaultValueSql("GETDATE()");

            builder.HasOne(r => r.Activo)
                   .WithMany(a => a.Reubicaciones)
                   .HasForeignKey(r => r.IdActivo);

            builder.HasOne(r => r.UsuarioAnterior)
                   .WithMany(u => u.ReubicacionesAnteriores)
                   .HasForeignKey(r => r.IdUsuarioAnterior)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.UsuarioNuevo)
                   .WithMany(u => u.ReubicacionesNuevas)
                   .HasForeignKey(r => r.IdUsuarioNuevo)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
