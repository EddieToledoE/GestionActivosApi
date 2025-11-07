using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GestionActivos.Domain.Entities;

namespace GestionActivos.Infrastructure.Persistence.Configurations
{
    public class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.ToTable("CAT_CATEGORIA");

            builder.HasKey(c => c.IdCategoria);

            builder.Property(c => c.Nombre)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(c => c.Descripcion)
                   .HasMaxLength(200);

            builder.Property(c => c.Activo)
                   .HasDefaultValue(true);

            builder.Property(c => c.FechaCreacion)
                   .HasDefaultValueSql("GETDATE()");
        }
    }
}
