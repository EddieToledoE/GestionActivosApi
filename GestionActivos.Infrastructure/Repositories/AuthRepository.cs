using GestionActivos.Domain.Entities;
using GestionActivos.Domain.Interfaces;
using GestionActivos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GestionActivos.Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _context;

        public AuthRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> LoginAsync(string correo, string contrasena)
        {
            // Cargar usuario con todas sus relaciones necesarias para el login
            var user = await _context.Usuarios
                .Include(u => u.CentroCosto) // Centro de costo principal (legacy)
                .Include(u => u.Roles) // Roles del usuario
                    .ThenInclude(ur => ur.Rol) // Información del rol
                        .ThenInclude(r => r.Permisos) // Permisos del rol
                            .ThenInclude(rp => rp.Permiso) // Información del permiso
                .Include(u => u.UsuarioCentrosCosto) // Centros de costo con acceso
                    .ThenInclude(ucc => ucc.CentroCosto) // Información del centro
                .FirstOrDefaultAsync(u => u.Correo == correo);

            if (user is null)
                return null;

            // Aquí puedes agregar lógica de hash o validación
            // Por ahora comparamos en texto plano (en producción usar BCrypt o similar)
            return user.Contrasena == contrasena ? user : null;
        }
    }
}
