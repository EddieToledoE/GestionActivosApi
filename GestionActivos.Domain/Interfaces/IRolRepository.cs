using GestionActivos.Domain.Entities;

namespace GestionActivos.Domain.Interfaces
{
    /// <summary>
    /// Repositorio para gestionar roles.
    /// </summary>
    public interface IRolRepository
    {
        Task<Rol?> GetByIdAsync(int id);
        Task<IEnumerable<Rol>> GetAllAsync();
        Task<Rol?> GetByNombreAsync(string nombre);
        Task<bool> ExistsByNombreAsync(string nombre);
        Task AddAsync(Rol rol);
        Task UpdateAsync(Rol rol);
        Task DeleteAsync(int id);
    }
}
