using GestionActivos.Domain.Entities;
using GestionActivos.Domain.Interfaces;
using GestionActivos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GestionActivos.Infrastructure.Repositories
{
    public class AuditoriaRepository : IAuditoriaRepository
    {
        private readonly ApplicationDbContext _context;

        public AuditoriaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Auditoria?> GetUltimaAuditoriaPorUsuarioAsync(Guid idUsuario)
        {
            // Obtiene la auditoría más reciente donde el usuario fue el auditado
            return await _context.Set<Auditoria>()
                .Include(a => a.Auditor)
                .Include(a => a.UsuarioAuditado)
                .Include(a => a.Detalles)
                .Where(a => a.IdUsuarioAuditado == idUsuario)
                .OrderByDescending(a => a.Fecha)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Auditoria>> GetAuditoriasPorUsuarioAsync(Guid idUsuario)
        {
            return await _context.Set<Auditoria>()
                .Include(a => a.Auditor)
                .Include(a => a.UsuarioAuditado)
                .Include(a => a.Detalles)
                .Where(a => a.IdUsuarioAuditado == idUsuario)
                .OrderByDescending(a => a.Fecha)
                .ToListAsync();
        }

        public async Task<IEnumerable<Auditoria>> GetAuditoriasPorCentroCostoAsync(int idCentroCosto)
        {
            return await _context.Set<Auditoria>()
                .Include(a => a.Auditor)
                .Include(a => a.UsuarioAuditado)
                .Include(a => a.CentroCosto)
                .Include(a => a.Detalles)
                    .ThenInclude(d => d.Activo)
                .Where(a => a.IdCentroCosto == idCentroCosto)
                .OrderByDescending(a => a.Fecha)
                .ToListAsync();
        }

        public async Task<Auditoria?> GetByIdAsync(Guid id)
        {
            return await _context.Set<Auditoria>()
                .Include(a => a.Auditor)
                .Include(a => a.UsuarioAuditado)
                .Include(a => a.CentroCosto)
                .Include(a => a.Detalles)
                    .ThenInclude(d => d.Activo)
                .FirstOrDefaultAsync(a => a.IdAuditoria == id);
        }

        public async Task AddAsync(Auditoria auditoria)
        {
            await _context.Set<Auditoria>().AddAsync(auditoria);
            // SaveChanges se maneja en Unit of Work
        }

        public Task UpdateAsync(Auditoria auditoria)
        {
            _context.Set<Auditoria>().Update(auditoria);
            // SaveChanges se maneja en Unit of Work
            return Task.CompletedTask;
        }
    }
}
