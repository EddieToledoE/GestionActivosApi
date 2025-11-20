namespace GestionActivos.Domain.Interfaces.UnitsOfWork
{
    /// <summary>
    /// Unidad de trabajo para el contexto de Solicitudes de transferencia de activos.
    /// Coordina las operaciones de: crear solicitud, aceptar, rechazar.
    /// </summary>
    public interface ISolicitudUnitOfWork : IDisposable
    {
        /// <summary>
        /// Repositorio de Solicitudes.
        /// </summary>
        ISolicitudRepository Solicitudes { get; }

        /// <summary>
        /// Repositorio de Activos (para validar existencia y actualizar responsable).
        /// </summary>
        IActivoRepository Activos { get; }

        /// <summary>
        /// Repositorio de Usuarios (para validar emisor, receptor, aprobador).
        /// </summary>
        IUsuarioRepository Usuarios { get; }

        /// <summary>
        /// Repositorio de Reubicaciones (para registrar el histórico al aceptar).
        /// </summary>
        IReubicacionRepository Reubicaciones { get; }

        /// <summary>
        /// Repositorio de Notificaciones (para notificar a los usuarios involucrados).
        /// </summary>
        INotificacionRepository Notificaciones { get; }

        /// <summary>
        /// Guarda todos los cambios pendientes en el contexto.
        /// </summary>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// Inicia una transacción explícita.
        /// </summary>
        Task BeginTransactionAsync();

        /// <summary>
        /// Confirma la transacción actual.
        /// </summary>
        Task CommitAsync();

        /// <summary>
        /// Revierte la transacción actual.
        /// </summary>
        Task RollbackAsync();
    }
}
