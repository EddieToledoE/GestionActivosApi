using GestionActivos.Domain.Entities;
using GestionActivos.Domain.Interfaces;
using GestionActivos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GestionActivos.Infrastructure.Repositories
{
    public class ActivoRepository : IActivoRepository
    {
        private readonly ApplicationDbContext _context;

        public ActivoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Activo?> GetByIdAsync(int id)
        {
            return await _context
                .Activos.Include(a => a.Responsable)
                .Include(a => a.CategoriaNavigation)
                .FirstOrDefaultAsync(a => a.IdActivo == id);
        }

        public async Task<IEnumerable<Activo>> GetAllAsync()
        {
            return await _context
                .Activos.Include(a => a.Responsable)
                .Include(a => a.CategoriaNavigation)
                .ToListAsync();
        }

        public async Task<IEnumerable<Activo>> GetByResponsableIdAsync(int responsableId)
        {
            return await _context
                .Activos.Include(a => a.Responsable)
                .Include(a => a.CategoriaNavigation)
                .Where(a => a.ResponsableId == responsableId)
                .ToListAsync();
        }

        public async Task AddAsync(Activo activo)
        {
            await _context.Activos.AddAsync(activo);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Activo activo)
        {
            _context.Activos.Update(activo);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var activo = await GetByIdAsync(id);
            if (activo != null)
            {
                activo.Estatus = "Inactivo";
                await UpdateAsync(activo);
            }
        }

        public async Task<bool> ExistsByEtiquetaAsync(string etiqueta)
        {
            return await _context.Activos.AnyAsync(a => a.Etiqueta == etiqueta);
        }

        public async Task<bool> ExistsByNumeroSerieAsync(string numeroSerie)
        {
            return await _context.Activos.AnyAsync(a => a.NumeroSerie == numeroSerie);
        }
    }
}
