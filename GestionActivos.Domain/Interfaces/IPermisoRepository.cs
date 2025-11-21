using GestionActivos.Domain.Entities;

namespace GestionActivos.Domain.Interfaces
{
    /// <summary>
    /// Repositorio para gestionar permisos.
    /// </summary>
    public interface IPermisoRepository
    {
        Task<Permiso?> GetByIdAsync(int id);
        Task<IEnumerable<Permiso>> GetAllAsync();
        Task<Permiso?> GetByNombreAsync(string nombre);
        Task<bool> ExistsByNombreAsync(string nombre);
        Task<IEnumerable<Permiso>> GetByIdsAsync(List<int> ids);
        Task AddAsync(Permiso permiso);
        Task UpdateAsync(Permiso permiso);
        Task DeleteAsync(int id);
    }
}
