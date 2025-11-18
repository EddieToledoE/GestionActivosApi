using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GestionActivos.Domain.Entities;

namespace GestionActivos.Infrastructure.Persistence.Configurations
{
    public class ConfigAuditoriaConfiguration : IEntityTypeConfiguration<ConfigAuditoria>
    {
        public void Configure(EntityTypeBuilder<ConfigAuditoria> builder)
        {
            builder.ToTable("CAT_CONFIG_AUDITORIA");
            builder.HasKey(c => c.IdConfig);

            builder.Property(c => c.Periodo)
                .HasMaxLength(20)
                .IsRequired()
                .HasDefaultValue("Mensual");

            builder.Property(c => c.Activa)
                .HasDefaultValue(true);

            builder.HasOne(c => c.CentroCosto)
                .WithMany(cc => cc.ConfiguracionesAuditoria)
                .HasForeignKey(c => c.IdCentroCosto)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Responsable)
                .WithMany()
                .HasForeignKey(c => c.IdResponsable)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
        }
    }
}
