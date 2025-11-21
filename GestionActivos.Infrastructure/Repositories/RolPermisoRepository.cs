using GestionActivos.Domain.Entities;
using GestionActivos.Domain.Interfaces;
using GestionActivos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GestionActivos.Infrastructure.Repositories
{
    public class RolPermisoRepository : IRolPermisoRepository
    {
        private readonly ApplicationDbContext _context;

        public RolPermisoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RolPermiso>> GetPermisosByRolIdAsync(int idRol)
        {
            return await _context.RolPermisos
                .Include(rp => rp.Permiso)
                .Where(rp => rp.IdRol == idRol)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(int idRol, int idPermiso)
        {
            return await _context.RolPermisos
                .AnyAsync(rp => rp.IdRol == idRol && rp.IdPermiso == idPermiso);
        }

        public async Task AddAsync(RolPermiso rolPermiso)
        {
            await _context.RolPermisos.AddAsync(rolPermiso);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(int idRol, int idPermiso)
        {
            var rolPermiso = await _context.RolPermisos
                .FirstOrDefaultAsync(rp => rp.IdRol == idRol && rp.IdPermiso == idPermiso);
            
            if (rolPermiso != null)
            {
                _context.RolPermisos.Remove(rolPermiso);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveAllByRolIdAsync(int idRol)
        {
            var rolPermisos = await _context.RolPermisos
                .Where(rp => rp.IdRol == idRol)
                .ToListAsync();
            
            _context.RolPermisos.RemoveRange(rolPermisos);
            await _context.SaveChangesAsync();
        }
    }
}
