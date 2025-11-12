using GestionActivos.Domain.Entities;
using GestionActivos.Domain.Interfaces;
using GestionActivos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GestionActivos.Infrastructure.Repositories
{
    public class SolicitudRepository : ISolicitudRepository
    {
        private readonly ApplicationDbContext _context;

        public SolicitudRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Solicitud?> GetByIdAsync(int id)
        {
            return await _context.Solicitudes
                .Include(s => s.Emisor)
                .Include(s => s.Receptor)
                .Include(s => s.Activo)
                .FirstOrDefaultAsync(s => s.IdSolicitud == id);
        }

        public async Task<IEnumerable<Solicitud>> GetAllAsync()
        {
            return await _context.Solicitudes
                .Include(s => s.Emisor)
                .Include(s => s.Receptor)
                .Include(s => s.Activo)
                .ToListAsync();
        }

        public async Task<IEnumerable<Solicitud>> GetByEmisorIdAsync(int emisorId)
        {
            return await _context.Solicitudes
                .Include(s => s.Emisor)
                .Include(s => s.Receptor)
                .Include(s => s.Activo)
                .Where(s => s.IdEmisor == emisorId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Solicitud>> GetByReceptorIdAsync(int receptorId)
        {
            return await _context.Solicitudes
                .Include(s => s.Emisor)
                .Include(s => s.Receptor)
                .Include(s => s.Activo)
                .Where(s => s.IdReceptor == receptorId)
                .ToListAsync();
        }

        public async Task AddAsync(Solicitud solicitud)
        {
            await _context.Solicitudes.AddAsync(solicitud);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Solicitud solicitud)
        {
            _context.Solicitudes.Update(solicitud);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var solicitud = await GetByIdAsync(id);
            if (solicitud != null)
            {
                _context.Solicitudes.Remove(solicitud);
                await _context.SaveChangesAsync();
            }
        }
    }
}
