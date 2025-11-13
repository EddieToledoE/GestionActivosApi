using GestionActivos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestionActivos.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        // DbSets
        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Rol> Roles => Set<Rol>();
        public DbSet<Permiso> Permisos => Set<Permiso>();
        public DbSet<UsuarioRol> UsuarioRoles => Set<UsuarioRol>();
        public DbSet<RolPermiso> RolPermisos => Set<RolPermiso>();
        public DbSet<Categoria> Categorias => Set<Categoria>();
        public DbSet<Activo> Activos => Set<Activo>();
        public DbSet<Reubicacion> Reubicaciones => Set<Reubicacion>();
        public DbSet<Diagnostico> Diagnosticos => Set<Diagnostico>();
        public DbSet<Solicitud> Solicitudes => Set<Solicitud>();
        public DbSet<ConfigAuditoria> ConfigAuditorias => Set<ConfigAuditoria>();
        public DbSet<Auditoria> Auditorias => Set<Auditoria>();
        public DbSet<DetalleAuditoria> DetallesAuditoria => Set<DetalleAuditoria>();
        public DbSet<CentroCosto> CentrosCosto => Set<CentroCosto>();
        public DbSet<Notificacion> Notificaciones => Set<Notificacion>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
