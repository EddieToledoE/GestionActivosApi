using GestionActivos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionActivos.Infrastructure.Persistence.Configurations
{
    public class MovNotificacionConfiguration : IEntityTypeConfiguration<Notificacion>
    {
        public void Configure(EntityTypeBuilder<Notificacion> builder)
        {
            builder.ToTable("MOV_NOTIFICACION");

            builder.HasKey(n => n.IdNotificacion);

            builder.Property(n => n.Origen).HasMaxLength(100).IsRequired(false);

            builder.Property(n => n.Tipo).HasMaxLength(50).IsRequired();

            builder.Property(n => n.Titulo).HasMaxLength(150).IsRequired();

            builder.Property(n => n.Mensaje).HasMaxLength(500).IsRequired();

            builder.Property(n => n.Fecha).HasDefaultValueSql("GETDATE()");

            builder.Property(n => n.Leida).HasDefaultValue(false);

            builder
                .HasOne(n => n.UsuarioDestino)
                .WithMany()
                .HasForeignKey(n => n.IdUsuarioDestino)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
