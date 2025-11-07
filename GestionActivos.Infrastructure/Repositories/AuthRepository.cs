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
            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == correo);

            if (user is null)
                return null;

            // Aquí puedes agregar lógica de hash o validación
            return user.Contrasena == contrasena ? user : null;
        }
    }
}
