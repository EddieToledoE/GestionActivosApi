using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GestionActivos.Domain.Entities;

namespace GestionActivos.Infrastructure.Persistence.Configurations
{
    public class RolConfiguration : IEntityTypeConfiguration<Rol>
    {
        public void Configure(EntityTypeBuilder<Rol> builder)
        {
            builder.ToTable("CAT_ROL");
            builder.HasKey(r => r.IdRol);

            builder.Property(r => r.Nombre)
                   .IsRequired()
                   .HasMaxLength(50);
        }
    }
}
