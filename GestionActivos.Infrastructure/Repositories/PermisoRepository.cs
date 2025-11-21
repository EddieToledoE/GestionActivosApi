using GestionActivos.Domain.Entities;
using GestionActivos.Domain.Interfaces;
using GestionActivos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GestionActivos.Infrastructure.Repositories
{
    public class PermisoRepository : IPermisoRepository
    {
        private readonly ApplicationDbContext _context;

        public PermisoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Permiso?> GetByIdAsync(int id)
        {
            return await _context.Permisos.FindAsync(id);
        }

        public async Task<IEnumerable<Permiso>> GetAllAsync()
        {
            return await _context.Permisos.ToListAsync();
        }

        public async Task<Permiso?> GetByNombreAsync(string nombre)
        {
            return await _context.Permisos
                .FirstOrDefaultAsync(p => p.Nombre.ToLower() == nombre.ToLower());
        }

        public async Task<bool> ExistsByNombreAsync(string nombre)
        {
            return await _context.Permisos
                .AnyAsync(p => p.Nombre.ToLower() == nombre.ToLower());
        }

        public async Task<IEnumerable<Permiso>> GetByIdsAsync(List<int> ids)
        {
            return await _context.Permisos
                .Where(p => ids.Contains(p.IdPermiso))
                .ToListAsync();
        }

        public async Task AddAsync(Permiso permiso)
        {
            await _context.Permisos.AddAsync(permiso);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Permiso permiso)
        {
            _context.Permisos.Update(permiso);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var permiso = await GetByIdAsync(id);
            if (permiso != null)
            {
                _context.Permisos.Remove(permiso);
                await _context.SaveChangesAsync();
            }
        }
    }
}
