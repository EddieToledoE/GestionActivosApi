using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GestionActivos.Domain.Entities;

namespace GestionActivos.Infrastructure.Persistence.Configurations
{
    public class RolPermisoConfiguration : IEntityTypeConfiguration<RolPermiso>
    {
        public void Configure(EntityTypeBuilder<RolPermiso> builder)
        {
            builder.ToTable("CAT_ROL_PERMISO");

            builder.HasKey(rp => new { rp.IdRol, rp.IdPermiso });

            builder.HasOne(rp => rp.Rol)
                   .WithMany(r => r.Permisos)
                   .HasForeignKey(rp => rp.IdRol);

            builder.HasOne(rp => rp.Permiso)
                   .WithMany(p => p.Roles)
                   .HasForeignKey(rp => rp.IdPermiso);
        }
    }
}
