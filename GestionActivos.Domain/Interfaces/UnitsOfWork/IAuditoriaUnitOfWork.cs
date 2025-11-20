namespace GestionActivos.Domain.Interfaces.UnitsOfWork
{
    /// <summary>
    /// Unidad de trabajo para el contexto de Auditorías.
    /// Coordina las operaciones de: crear auditoría con sus detalles.
    /// </summary>
    public interface IAuditoriaUnitOfWork : IDisposable
    {
        /// <summary>
        /// Repositorio de Auditorías.
        /// </summary>
        IAuditoriaRepository Auditorias { get; }

        /// <summary>
        /// Repositorio de Usuarios (para validar auditor y usuario auditado).
        /// </summary>
        IUsuarioRepository Usuarios { get; }

        /// <summary>
        /// Repositorio de Activos (para validar activos en los detalles).
        /// </summary>
        IActivoRepository Activos { get; }

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
