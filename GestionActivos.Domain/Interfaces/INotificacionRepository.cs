using GestionActivos.Domain.Entities;

namespace GestionActivos.Domain.Interfaces
{
    /// <summary>
    /// Repositorio para gestionar las notificaciones de usuarios.
    /// </summary>
    public interface INotificacionRepository
    {
        /// <summary>
        /// Obtiene una notificación por su ID.
        /// </summary>
        Task<Notificacion?> GetByIdAsync(int id);

        /// <summary>
        /// Obtiene todas las notificaciones.
        /// </summary>
        Task<IEnumerable<Notificacion>> GetAllAsync();

        /// <summary>
        /// Obtiene todas las notificaciones de un usuario específico.
        /// </summary>
        Task<IEnumerable<Notificacion>> GetByUsuarioIdAsync(Guid usuarioId);

        /// <summary>
        /// Obtiene las notificaciones no leídas de un usuario.
        /// </summary>
        Task<IEnumerable<Notificacion>> GetNoLeidasByUsuarioIdAsync(Guid usuarioId);

        /// <summary>
        /// Agrega una nueva notificación.
        /// </summary>
        Task AddAsync(Notificacion notificacion);

        /// <summary>
        /// Actualiza una notificación existente.
        /// </summary>
        Task UpdateAsync(Notificacion notificacion);

        /// <summary>
        /// Marca una notificación como leída.
        /// </summary>
        Task MarcarComoLeidaAsync(int id);

        /// <summary>
        /// Elimina una notificación.
        /// </summary>
        Task DeleteAsync(int id);
    }
}
