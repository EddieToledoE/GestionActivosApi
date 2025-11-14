using GestionActivos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionActivos.Infrastructure.Persistence.Configurations
{
    public class DiagnosticoConfiguration : IEntityTypeConfiguration<Diagnostico>
    {
        public void Configure(EntityTypeBuilder<Diagnostico> builder)
        {
            builder.ToTable("MOV_DIAGNOSTICO");
            builder.HasKey(d => d.IdDiagnostico);

            builder.Property(d => d.Tipo).HasColumnType("nvarchar(50)").IsRequired();
            builder.Property(d => d.Observaciones).HasColumnType("nvarchar(max)");
            builder.Property(d => d.Fecha).HasDefaultValueSql("GETDATE()");

            builder
                .HasOne(d => d.Activo)
                .WithMany(a => a.Diagnosticos)
                .HasForeignKey(d => d.IdActivo);

            builder
                .HasOne(d => d.Tecnico)
                .WithMany(u => u.Diagnosticos)
                .HasForeignKey(d => d.IdTecnico)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
