using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GestionActivos.Domain.Entities;

namespace GestionActivos.Infrastructure.Persistence.Configurations
{
    public class SolicitudConfiguration : IEntityTypeConfiguration<Solicitud>
    {
        public void Configure(EntityTypeBuilder<Solicitud> builder)
        {
            builder.ToTable("MOV_SOLICITUD");
            builder.HasKey(s => s.IdSolicitud);

            builder.Property(s => s.Tipo).HasMaxLength(50);
            builder.Property(s => s.Mensaje).HasMaxLength(300);
            builder.Property(s => s.Estado)
                   .HasMaxLength(20)
                   .HasDefaultValue("Pendiente");
            builder.Property(s => s.Fecha).HasDefaultValueSql("GETDATE()");

            builder.HasOne(s => s.Emisor)
                   .WithMany(u => u.SolicitudesEmitidas)
                   .HasForeignKey(s => s.IdEmisor)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Receptor)
                   .WithMany(u => u.SolicitudesRecibidas)
                   .HasForeignKey(s => s.IdReceptor)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Activo)
                   .WithMany()
                   .HasForeignKey(s => s.IdActivo)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
