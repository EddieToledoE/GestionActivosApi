using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GestionActivos.Domain.Entities;

namespace GestionActivos.Infrastructure.Persistence.Configurations
{
    public class UsuarioRolConfiguration : IEntityTypeConfiguration<UsuarioRol>
    {
        public void Configure(EntityTypeBuilder<UsuarioRol> builder)
        {
            builder.ToTable("MOV_USUARIO_ROL");

            builder.HasKey(ur => new { ur.IdUsuario, ur.IdRol });

            builder.HasOne(ur => ur.Usuario)
                   .WithMany(u => u.Roles)
                   .HasForeignKey(ur => ur.IdUsuario);

            builder.HasOne(ur => ur.Rol)
                   .WithMany(r => r.Usuarios)
                   .HasForeignKey(ur => ur.IdRol);
        }
    }
}
