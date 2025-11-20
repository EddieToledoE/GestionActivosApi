namespace GestionActivos.Domain.Interfaces.UnitsOfWork
{
    /// <summary>
    /// Unidad de trabajo para el contexto de Transferencias directas por Auditor.
    /// Coordina las operaciones de: transferir activo directamente sin solicitud previa.
    /// </summary>
    public interface ITransferenciaUnitOfWork : IDisposable
    {
        /// <summary>
        /// Repositorio de Activos (para actualizar responsable).
        /// </summary>
        IActivoRepository Activos { get; }

        /// <summary>
        /// Repositorio de Usuarios (para validar auditor y usuario destino).
        /// </summary>
        IUsuarioRepository Usuarios { get; }

        /// <summary>
        /// Repositorio de Reubicaciones (para registrar el histórico de transferencia).
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
