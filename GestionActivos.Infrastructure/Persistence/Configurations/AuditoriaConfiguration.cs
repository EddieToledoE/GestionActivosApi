using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GestionActivos.Domain.Entities;

namespace GestionActivos.Infrastructure.Persistence.Configurations
{
    public class AuditoriaConfiguration : IEntityTypeConfiguration<Auditoria>
    {
        public void Configure(EntityTypeBuilder<Auditoria> builder)
        {
            builder.ToTable("MOV_AUDITORIA");
            builder.HasKey(a => a.IdAuditoria);

            builder.Property(a => a.Tipo).HasMaxLength(20);
            builder.Property(a => a.Observaciones).HasColumnType("nvarchar(max)");
            builder.Property(a => a.Fecha).HasDefaultValueSql("GETDATE()");

            builder.HasOne(a => a.Auditor)
                   .WithMany()
                   .HasForeignKey(a => a.IdAuditor)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.UsuarioAuditado)
                   .WithMany()
                   .HasForeignKey(a => a.IdUsuarioAuditado)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.CentroCosto)
                   .WithMany()
                   .HasForeignKey(a => a.IdCentroCosto)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
