using GestionActivos.Domain.Entities;

namespace GestionActivos.Domain.Interfaces
{
    /// <summary>
    /// Repositorio para gestionar la relación muchos-a-muchos entre Usuario y CentroCosto.
    /// </summary>
    public interface IUsuarioCentroCostoRepository
    {
        /// <summary>
        /// Obtiene todos los centros de costo asignados a un usuario.
        /// </summary>
        Task<IEnumerable<UsuarioCentroCosto>> GetByUsuarioIdAsync(Guid idUsuario);

        /// <summary>
        /// Obtiene todos los usuarios asignados a un centro de costo.
        /// </summary>
        Task<IEnumerable<UsuarioCentroCosto>> GetByCentroCostoIdAsync(int idCentroCosto);

        /// <summary>
        /// Obtiene una relación específica entre usuario y centro de costo.
        /// </summary>
        Task<UsuarioCentroCosto?> GetByIdAsync(Guid idUsuario, int idCentroCosto);

        /// <summary>
        /// Verifica si existe una relación activa entre usuario y centro de costo.
        /// </summary>
        Task<bool> ExistsAsync(Guid idUsuario, int idCentroCosto);

        /// <summary>
        /// Agrega una nueva asignación de usuario a centro de costo.
        /// </summary>
        Task AddAsync(UsuarioCentroCosto entity);

        /// <summary>
        /// Actualiza una asignación existente.
        /// </summary>
        Task UpdateAsync(UsuarioCentroCosto entity);

        /// <summary>
        /// Elimina una asignación (soft delete marcando como inactivo).
        /// </summary>
        Task RemoveAsync(Guid idUsuario, int idCentroCosto);
    }
}
