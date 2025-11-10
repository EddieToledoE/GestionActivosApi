using GestionActivos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionActivos.Infrastructure.Persistence.Configurations
{
    public class CentroCostoConfiguration : IEntityTypeConfiguration<CentroCosto>
    {
        public void Configure(EntityTypeBuilder<CentroCosto> builder)
        {
            builder.ToTable("CAT_CENTRO_COSTO");

            builder.HasKey(c => c.IdCentroCosto);

            builder.Property(c => c.Ubicacion).HasMaxLength(150);

            builder.Property(c => c.RazonSocial).HasMaxLength(150);

            builder.Property(c => c.Area).HasMaxLength(100);

            builder.Property(c => c.Activo).HasDefaultValue(true);

            builder.Property(c => c.FechaCreacion).HasDefaultValueSql("GETDATE()");
        }
    }
}
