using GestionActivos.Domain.Entities;

namespace GestionActivos.Domain.Interfaces
{
    /// <summary>
    /// Repositorio para gestionar las configuraciones de auditoría por centro de costo.
    /// </summary>
    public interface IConfigAuditoriaRepository
    {
        /// <summary>
        /// Obtiene la configuración activa de auditoría para un centro de costo específico.
        /// </summary>
        Task<ConfigAuditoria?> GetConfiguracionActivaPorCentroCostoAsync(int idCentroCosto);

        /// <summary>
        /// Obtiene todas las configuraciones de auditoría.
        /// </summary>
        Task<IEnumerable<ConfigAuditoria>> GetAllAsync();

        /// <summary>
        /// Obtiene una configuración por su ID.
        /// </summary>
        Task<ConfigAuditoria?> GetByIdAsync(int id);

        /// <summary>
        /// Agrega una nueva configuración de auditoría.
        /// </summary>
        Task AddAsync(ConfigAuditoria config);

        /// <summary>
        /// Actualiza una configuración existente.
        /// </summary>
        Task UpdateAsync(ConfigAuditoria config);
    }
}
