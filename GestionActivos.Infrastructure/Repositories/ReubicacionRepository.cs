using GestionActivos.Domain.Entities;
using GestionActivos.Domain.Interfaces;
using GestionActivos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GestionActivos.Infrastructure.Repositories
{
    public class ReubicacionRepository : IReubicacionRepository
    {
        private readonly ApplicationDbContext _context;

        public ReubicacionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Reubicacion?> GetByIdAsync(int id)
        {
            return await _context.Reubicaciones
                .Include(r => r.Activo)
                .Include(r => r.UsuarioAnterior)
                .Include(r => r.UsuarioNuevo)
                .FirstOrDefaultAsync(r => r.IdReubicacion == id);
        }

        public async Task<IEnumerable<Reubicacion>> GetAllAsync()
        {
            return await _context.Reubicaciones
                .Include(r => r.Activo)
                .Include(r => r.UsuarioAnterior)
                .Include(r => r.UsuarioNuevo)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reubicacion>> GetByActivoIdAsync(Guid activoId)
        {
            return await _context.Reubicaciones
                .Include(r => r.Activo)
                .Include(r => r.UsuarioAnterior)
                .Include(r => r.UsuarioNuevo)
                .Where(r => r.IdActivo == activoId)
                .OrderByDescending(r => r.Fecha)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reubicacion>> GetByUsuarioAsync(Guid usuarioId)
        {
            return await _context.Reubicaciones
                .Include(r => r.Activo)
                .Include(r => r.UsuarioAnterior)
                .Include(r => r.UsuarioNuevo)
                .Where(r => r.IdUsuarioAnterior == usuarioId || r.IdUsuarioNuevo == usuarioId)
                .OrderByDescending(r => r.Fecha)
                .ToListAsync();
        }

        public async Task AddAsync(Reubicacion reubicacion)
        {
            await _context.Reubicaciones.AddAsync(reubicacion);
        }

        public Task UpdateAsync(Reubicacion reubicacion)
        {
            _context.Reubicaciones.Update(reubicacion);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id)
        {
            var reubicacion = _context.Reubicaciones.Find(id);
            if (reubicacion != null)
            {
                _context.Reubicaciones.Remove(reubicacion);
            }
            return Task.CompletedTask;
        }
    }
}
