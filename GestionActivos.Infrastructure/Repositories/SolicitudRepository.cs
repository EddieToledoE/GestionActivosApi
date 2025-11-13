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

        public async Task<bool> ExisteSolicitudPendienteParaActivoAsync(int idActivo)
        {
            return await _context.Solicitudes
                .AnyAsync(s => s.IdActivo == idActivo && s.Estado == "Pendiente");
        }

        public async Task AddAsync(Solicitud solicitud)
        {
            await _context.Solicitudes.AddAsync(solicitud);
            // No llamar a SaveChangesAsync aquí, lo maneja el UoW o el llamador
        }

        public Task UpdateAsync(Solicitud solicitud)
        {
            _context.Solicitudes.Update(solicitud);
            // No llamar a SaveChangesAsync aquí, lo maneja el UoW o el llamador
            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id)
        {
            var solicitud = _context.Solicitudes.Find(id);
            if (solicitud != null)
            {
                _context.Solicitudes.Remove(solicitud);
                // No llamar a SaveChangesAsync aquí, lo maneja el UoW o el llamador
            }
            return Task.CompletedTask;
        }
    }
}
