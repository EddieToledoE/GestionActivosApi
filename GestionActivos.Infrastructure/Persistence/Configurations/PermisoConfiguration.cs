using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GestionActivos.Domain.Entities;

namespace GestionActivos.Infrastructure.Persistence.Configurations
{
    public class PermisoConfiguration : IEntityTypeConfiguration<Permiso>
    {
        public void Configure(EntityTypeBuilder<Permiso> builder)
        {
            builder.ToTable("CAT_PERMISO");
            builder.HasKey(p => p.IdPermiso);

            builder.Property(p => p.Nombre)
                   .IsRequired()
                   .HasMaxLength(100);
        }
    }
}
