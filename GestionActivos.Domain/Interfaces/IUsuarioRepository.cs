using System.Collections.Generic;
using System.Threading.Tasks;
using GestionActivos.Domain.Entities;

namespace GestionActivos.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> GetByIdAsync(int id);
        Task<Usuario?> GetByCorreoAsync(string correo);
        Task<IEnumerable<Usuario>> GetAllAsync();
        Task AddAsync(Usuario usuario);
        Task UpdateAsync(Usuario usuario);
        Task<bool> ExistsByCorreoAsync(string correo);
    }
}
