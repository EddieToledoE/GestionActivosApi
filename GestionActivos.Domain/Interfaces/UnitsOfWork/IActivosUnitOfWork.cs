namespace GestionActivos.Domain.Interfaces.UnitsOfWork
{
    /// <summary>
    /// Unidad de trabajo para el contexto de Activos, que coordina múltiples repositorios
    /// y maneja transacciones de manera centralizada.
    /// </summary>
    public interface IActivosUnitOfWork : IDisposable
    {
        /// <summary>
        /// Repositorio de Activos.
        /// </summary>
        IActivoRepository Activos { get; }

        /// <summary>
        /// Repositorio de Solicitudes.
        /// </summary>
        ISolicitudRepository Solicitudes { get; }

        /// <summary>
        /// Repositorio de Reubicaciones.
        /// </summary>
        IReubicacionRepository Reubicaciones { get; }

        /// <summary>
        /// Repositorio de Usuarios.
        /// </summary>
        IUsuarioRepository Usuarios { get; }

        /// <summary>
        /// Repositorio de Notificaciones.
        /// </summary>
        INotificacionRepository Notificaciones { get; }

        /// <summary>
        /// Repositorio de Auditorías.
        /// </summary>
        IAuditoriaRepository Auditorias { get; }

        /// <summary>
        /// Guarda todos los cambios pendientes en el contexto.
        /// </summary>
        /// <returns>Número de entidades afectadas.</returns>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// Inicia una transacción explícita en la base de datos.
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
