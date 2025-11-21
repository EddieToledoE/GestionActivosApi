using GestionActivos.Domain.Entities;
using GestionActivos.Domain.Interfaces;
using GestionActivos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GestionActivos.Infrastructure.Repositories
{
    public class RolRepository : IRolRepository
    {
        private readonly ApplicationDbContext _context;

        public RolRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Rol?> GetByIdAsync(int id)
        {
            return await _context.Roles
                .Include(r => r.Permisos)
                    .ThenInclude(rp => rp.Permiso)
                .FirstOrDefaultAsync(r => r.IdRol == id);
        }

        public async Task<IEnumerable<Rol>> GetAllAsync()
        {
            return await _context.Roles
                .Include(r => r.Permisos)
                    .ThenInclude(rp => rp.Permiso)
                .ToListAsync();
        }

        public async Task<Rol?> GetByNombreAsync(string nombre)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(r => r.Nombre.ToLower() == nombre.ToLower());
        }

        public async Task<bool> ExistsByNombreAsync(string nombre)
        {
            return await _context.Roles
                .AnyAsync(r => r.Nombre.ToLower() == nombre.ToLower());
        }

        public async Task AddAsync(Rol rol)
        {
            await _context.Roles.AddAsync(rol);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Rol rol)
        {
            _context.Roles.Update(rol);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var rol = await GetByIdAsync(id);
            if (rol != null)
            {
                _context.Roles.Remove(rol);
                await _context.SaveChangesAsync();
            }
        }
    }
}
