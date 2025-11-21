using GestionActivos.Domain.Entities;
using GestionActivos.Domain.Interfaces;
using GestionActivos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GestionActivos.Infrastructure.Repositories
{
    /// <summary>
    /// Implementación del repositorio para UsuarioCentroCosto.
    /// </summary>
    public class UsuarioCentroCostoRepository : IUsuarioCentroCostoRepository
    {
        private readonly ApplicationDbContext _context;

        public UsuarioCentroCostoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UsuarioCentroCosto>> GetByUsuarioIdAsync(Guid idUsuario)
        {
            return await _context.UsuarioCentrosCosto
                .Include(uc => uc.CentroCosto)
                .Include(uc => uc.Usuario)
                .Where(uc => uc.IdUsuario == idUsuario)
                .OrderByDescending(uc => uc.Activo)
                .ThenByDescending(uc => uc.FechaInicio)
                .ToListAsync();
        }

        public async Task<IEnumerable<UsuarioCentroCosto>> GetByCentroCostoIdAsync(int idCentroCosto)
        {
            return await _context.UsuarioCentrosCosto
                .Include(uc => uc.Usuario)
                .Include(uc => uc.CentroCosto)
                .Where(uc => uc.IdCentroCosto == idCentroCosto)
                .OrderByDescending(uc => uc.Activo)
                .ThenByDescending(uc => uc.FechaInicio)
                .ToListAsync();
        }

        public async Task<UsuarioCentroCosto?> GetByIdAsync(Guid idUsuario, int idCentroCosto)
        {
            return await _context.UsuarioCentrosCosto
                .Include(uc => uc.Usuario)
                .Include(uc => uc.CentroCosto)
                .FirstOrDefaultAsync(uc => uc.IdUsuario == idUsuario && uc.IdCentroCosto == idCentroCosto);
        }

        public async Task<bool> ExistsAsync(Guid idUsuario, int idCentroCosto)
        {
            return await _context.UsuarioCentrosCosto
                .AnyAsync(uc => uc.IdUsuario == idUsuario && uc.IdCentroCosto == idCentroCosto && uc.Activo);
        }

        public async Task AddAsync(UsuarioCentroCosto entity)
        {
            await _context.UsuarioCentrosCosto.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(UsuarioCentroCosto entity)
        {
            _context.UsuarioCentrosCosto.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(Guid idUsuario, int idCentroCosto)
        {
            var entity = await GetByIdAsync(idUsuario, idCentroCosto);
            if (entity != null)
            {
                entity.Activo = false;
                entity.FechaFin = DateTime.Now;
                await UpdateAsync(entity);
            }
        }
    }
}
