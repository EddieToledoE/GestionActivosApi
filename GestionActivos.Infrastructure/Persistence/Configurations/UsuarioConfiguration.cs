using GestionActivos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionActivos.Infrastructure.Persistence.Configurations
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("CAT_USUARIO");

            builder.HasKey(u => u.IdUsuario);

            builder.Property(u => u.Nombres).IsRequired().HasMaxLength(100);

            builder.Property(u => u.ApellidoPaterno).IsRequired().HasMaxLength(100);

            builder.Property(u => u.ApellidoMaterno).HasMaxLength(100);

            builder.Property(u => u.ClaveFortia).HasMaxLength(50);

            builder.HasIndex(u => u.ClaveFortia).IsUnique();

            builder.Property(u => u.Correo).HasMaxLength(100);

            builder.Property(u => u.Contrasena).IsRequired().HasMaxLength(200);

            builder.Property(u => u.Activo).HasDefaultValue(true);

            builder
                .HasOne(u => u.CentroCosto)
                .WithMany(c => c.Usuarios)
                .HasForeignKey(u => u.IdCentroCosto)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
