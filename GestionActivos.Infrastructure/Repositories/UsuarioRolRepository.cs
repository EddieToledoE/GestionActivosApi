using GestionActivos.Domain.Entities;
using GestionActivos.Domain.Interfaces;
using GestionActivos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GestionActivos.Infrastructure.Repositories
{
    public class UsuarioRolRepository : IUsuarioRolRepository
    {
        private readonly ApplicationDbContext _context;

        public UsuarioRolRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UsuarioRol>> GetRolesByUsuarioIdAsync(Guid idUsuario)
        {
            return await _context.UsuarioRoles
                .Include(ur => ur.Rol)
                    .ThenInclude(r => r.Permisos)
                        .ThenInclude(rp => rp.Permiso)
                .Where(ur => ur.IdUsuario == idUsuario)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(Guid idUsuario, int idRol)
        {
            return await _context.UsuarioRoles
                .AnyAsync(ur => ur.IdUsuario == idUsuario && ur.IdRol == idRol);
        }

        public async Task AddAsync(UsuarioRol usuarioRol)
        {
            await _context.UsuarioRoles.AddAsync(usuarioRol);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(Guid idUsuario, int idRol)
        {
            var usuarioRol = await _context.UsuarioRoles
                .FirstOrDefaultAsync(ur => ur.IdUsuario == idUsuario && ur.IdRol == idRol);
            
            if (usuarioRol != null)
            {
                _context.UsuarioRoles.Remove(usuarioRol);
                await _context.SaveChangesAsync();
            }
        }
    }
}
