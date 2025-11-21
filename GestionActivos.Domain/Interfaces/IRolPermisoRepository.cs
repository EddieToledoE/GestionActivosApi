using GestionActivos.Domain.Entities;

namespace GestionActivos.Domain.Interfaces
{
    /// <summary>
    /// Repositorio para gestionar la relación Rol-Permiso.
    /// </summary>
    public interface IRolPermisoRepository
    {
        Task<IEnumerable<RolPermiso>> GetPermisosByRolIdAsync(int idRol);
        Task<bool> ExistsAsync(int idRol, int idPermiso);
        Task AddAsync(RolPermiso rolPermiso);
        Task RemoveAsync(int idRol, int idPermiso);
        Task RemoveAllByRolIdAsync(int idRol);
    }
}
