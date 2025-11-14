using GestionActivos.Domain.Entities;
using GestionActivos.Domain.Interfaces;
using GestionActivos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GestionActivos.Infrastructure.Repositories
{
    /// <summary>
    /// Implementación del repositorio de notificaciones.
    /// </summary>
    public class NotificacionRepository : INotificacionRepository
    {
        private readonly ApplicationDbContext _context;

        public NotificacionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Notificacion?> GetByIdAsync(int id)
        {
            return await _context.Notificaciones
                .Include(n => n.UsuarioDestino)
                .FirstOrDefaultAsync(n => n.IdNotificacion == id);
        }

        public async Task<IEnumerable<Notificacion>> GetAllAsync()
        {
            return await _context.Notificaciones
                .Include(n => n.UsuarioDestino)
                .OrderByDescending(n => n.Fecha)
                .ToListAsync();
        }

        public async Task<IEnumerable<Notificacion>> GetByUsuarioIdAsync(Guid usuarioId)
        {
            return await _context.Notificaciones
                .Include(n => n.UsuarioDestino)
                .Where(n => n.IdUsuarioDestino == usuarioId)
                .OrderByDescending(n => n.Fecha)
                .ToListAsync();
        }

        public async Task<IEnumerable<Notificacion>> GetNoLeidasByUsuarioIdAsync(Guid usuarioId)
        {
            return await _context.Notificaciones
                .Include(n => n.UsuarioDestino)
                .Where(n => n.IdUsuarioDestino == usuarioId && !n.Leida)
                .OrderByDescending(n => n.Fecha)
                .ToListAsync();
        }

        public async Task AddAsync(Notificacion notificacion)
        {
            await _context.Notificaciones.AddAsync(notificacion);
            // No llamar SaveChangesAsync aquí, lo maneja el UoW
        }

        public Task UpdateAsync(Notificacion notificacion)
        {
            _context.Notificaciones.Update(notificacion);
            // No llamar SaveChangesAsync aquí, lo maneja el UoW
            return Task.CompletedTask;
        }

        public async Task MarcarComoLeidaAsync(int id)
        {
            var notificacion = await GetByIdAsync(id);
            if (notificacion != null)
            {
                notificacion.Leida = true;
                _context.Notificaciones.Update(notificacion);
                // No llamar SaveChangesAsync aquí, lo maneja el UoW
            }
        }

        public async Task DeleteAsync(int id)
        {
            var notificacion = await GetByIdAsync(id);
            if (notificacion != null)
            {
                _context.Notificaciones.Remove(notificacion);
                // No llamar SaveChangesAsync aquí, lo maneja el UoW
            }
        }
    }
}
