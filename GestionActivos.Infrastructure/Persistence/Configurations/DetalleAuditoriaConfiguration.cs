using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GestionActivos.Domain.Entities;

namespace GestionActivos.Infrastructure.Persistence.Configurations
{
    public class DetalleAuditoriaConfiguration : IEntityTypeConfiguration<DetalleAuditoria>
    {
        public void Configure(EntityTypeBuilder<DetalleAuditoria> builder)
        {
            builder.ToTable("H_DETALLE_AUDITORIA");
            builder.HasKey(d => d.IdDetalle);

            builder.Property(d => d.Estado)
                   .HasMaxLength(50);

            builder.Property(d => d.Comentarios)
                   .HasMaxLength(200);

            builder.HasOne(d => d.Auditoria)
                   .WithMany(a => a.Detalles)
                   .HasForeignKey(d => d.IdAuditoria);

            builder.HasOne(d => d.Activo)
                   .WithMany(a => a.DetallesAuditoria)
                   .HasForeignKey(d => d.IdActivo);
        }
    }
}
