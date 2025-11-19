using GestionActivos.Domain.Entities;
using GestionActivos.Domain.Interfaces;
using GestionActivos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GestionActivos.Infrastructure.Repositories
{
    public class ConfigAuditoriaRepository : IConfigAuditoriaRepository
    {
        private readonly ApplicationDbContext _context;

        public ConfigAuditoriaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ConfigAuditoria?> GetConfiguracionActivaPorCentroCostoAsync(int idCentroCosto)
        {
            return await _context.Set<ConfigAuditoria>()
                .Include(c => c.CentroCosto)
                .Include(c => c.Responsable)
                .FirstOrDefaultAsync(c => c.IdCentroCosto == idCentroCosto && c.Activa);
        }

        public async Task<IEnumerable<ConfigAuditoria>> GetAllAsync()
        {
            return await _context.Set<ConfigAuditoria>()
                .Include(c => c.CentroCosto)
                .Include(c => c.Responsable)
                .ToListAsync();
        }

        public async Task<ConfigAuditoria?> GetByIdAsync(int id)
        {
            return await _context.Set<ConfigAuditoria>()
                .Include(c => c.CentroCosto)
                .Include(c => c.Responsable)
                .FirstOrDefaultAsync(c => c.IdConfig == id);
        }

        public async Task AddAsync(ConfigAuditoria config)
        {
            await _context.Set<ConfigAuditoria>().AddAsync(config);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ConfigAuditoria config)
        {
            _context.Set<ConfigAuditoria>().Update(config);
            await _context.SaveChangesAsync();
        }
    }
}
