using GestionActivos.Domain.Entities;
using GestionActivos.Domain.Interfaces;
using GestionActivos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GestionActivos.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ApplicationDbContext _context;

        public UsuarioRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> GetByIdAsync(Guid id) =>
            await _context.Usuarios
                .Include(u => u.CentroCosto)
                .FirstOrDefaultAsync(u => u.IdUsuario == id);

        public async Task<Usuario?> GetByCorreoAsync(string correo) =>
            await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == correo);

        public async Task<Usuario?> GetByClaveFortiaAsync(string claveFortia) =>
            await _context.Usuarios
                .Include(u => u.CentroCosto)
                .FirstOrDefaultAsync(u => u.ClaveFortia == claveFortia);

        public async Task<IEnumerable<Usuario>> GetAllAsync() =>
            await _context.Usuarios
                .Include(u => u.CentroCosto)
                .ToListAsync();

        public async Task<IEnumerable<Usuario>> GetUsuariosByCentrosCostoAsync(List<int> idsCentrosCosto)
        {
            return await _context.Usuarios
                .Include(u => u.CentroCosto)
                .Where(u => u.IdCentroCosto.HasValue && idsCentrosCosto.Contains(u.IdCentroCosto.Value))
                .Where(u => u.Activo) // Solo usuarios activos
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddAsync(Usuario usuario)
        {
            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var usuario = await GetByIdAsync(id);
            if (usuario != null)
            {
                usuario.Activo = false;
                await UpdateAsync(usuario);
            }
        }

        public async Task<bool> ExistsByCorreoAsync(string correo) =>
            await _context.Usuarios.AnyAsync(u => u.Correo == correo);
    }
}
