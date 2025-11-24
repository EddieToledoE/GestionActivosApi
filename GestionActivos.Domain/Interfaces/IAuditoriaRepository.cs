using GestionActivos.Domain.Entities;

namespace GestionActivos.Domain.Interfaces
{
    /// <summary>
    /// Repositorio para gestionar las auditorías realizadas.
    /// </summary>
    public interface IAuditoriaRepository
    {
        /// <summary>
        /// Obtiene la última auditoría realizada por un usuario específico.
        /// </summary>
        Task<Auditoria?> GetUltimaAuditoriaPorUsuarioAsync(Guid idUsuario);

        /// <summary>
        /// Obtiene todas las auditorías de un usuario.
        /// </summary>
        Task<IEnumerable<Auditoria>> GetAuditoriasPorUsuarioAsync(Guid idUsuario);

        /// <summary>
        /// Obtiene todas las auditorías de un centro de costo específico.
        /// </summary>
        Task<IEnumerable<Auditoria>> GetAuditoriasPorCentroCostoAsync(int idCentroCosto);

        /// <summary>
        /// Obtiene todas las auditorías de múltiples centros de costo.
        /// </summary>
        Task<IEnumerable<Auditoria>> GetAuditoriasByCentrosCostoAsync(List<int> idsCentrosCosto);

        /// <summary>
        /// Obtiene una auditoría por su ID.
        /// </summary>
        Task<Auditoria?> GetByIdAsync(Guid id);

        /// <summary>
        /// Agrega una nueva auditoría.
        /// </summary>
        Task AddAsync(Auditoria auditoria);

        /// <summary>
        /// Actualiza una auditoría existente.
        /// </summary>
        Task UpdateAsync(Auditoria auditoria);
    }
}
