using GestionActivos.Domain.Interfaces;
using GestionActivos.Domain.Interfaces.UnitsOfWork;
using GestionActivos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Storage;

namespace GestionActivos.Infrastructure.UnitsOfWork
{
    /// <summary>
    /// Implementación del patrón Unit of Work para coordinar operaciones
    /// sobre múltiples repositorios relacionados con Activos.
    /// </summary>
    public class ActivosUnitOfWork : IActivosUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _transaction;

        public ActivosUnitOfWork(
            ApplicationDbContext context,
            IActivoRepository activoRepository,
            ISolicitudRepository solicitudRepository,
            IReubicacionRepository reubicacionRepository,
            IUsuarioRepository usuarioRepository,
            INotificacionRepository notificacionRepository,
            IAuditoriaRepository auditoriaRepository
        )
        {
            _context = context;
            Activos = activoRepository;
            Solicitudes = solicitudRepository;
            Reubicaciones = reubicacionRepository;
            Usuarios = usuarioRepository;
            Notificaciones = notificacionRepository;
            Auditorias = auditoriaRepository;
        }

        /// <summary>
        /// Repositorio de Activos.
        /// </summary>
        public IActivoRepository Activos { get; }

        /// <summary>
        /// Repositorio de Solicitudes.
        /// </summary>
        public ISolicitudRepository Solicitudes { get; }

        /// <summary>
        /// Repositorio de Reubicaciones.
        /// </summary>
        public IReubicacionRepository Reubicaciones { get; }

        /// <summary>
        /// Repositorio de Usuarios.
        /// </summary>
        public IUsuarioRepository Usuarios { get; }

        /// <summary>
        /// Repositorio de Notificaciones.
        /// </summary>
        public INotificacionRepository Notificaciones { get; }

        /// <summary>
        /// Repositorio de Auditorías.
        /// </summary>
        public IAuditoriaRepository Auditorias { get; }

        /// <summary>
        /// Guarda todos los cambios pendientes en el contexto de la base de datos.
        /// </summary>
        /// <returns>El número de entidades afectadas.</returns>
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Inicia una transacción explícita en la base de datos.
        /// Permite realizar múltiples operaciones de forma atómica.
        /// </summary>
        public async Task BeginTransactionAsync()
        {
            if (_transaction != null)
            {
                throw new InvalidOperationException("Ya existe una transacción activa.");
            }

            _transaction = await _context.Database.BeginTransactionAsync();
        }

        /// <summary>
        /// Confirma la transacción actual, aplicando todos los cambios a la base de datos.
        /// </summary>
        public async Task CommitAsync()
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("No hay una transacción activa para confirmar.");
            }

            try
            {
                await _transaction.CommitAsync();
            }
            catch
            {
                await RollbackAsync();
                throw;
            }
            finally
            {
                await DisposeTransactionAsync();
            }
        }

        /// <summary>
        /// Revierte la transacción actual, descartando todos los cambios pendientes.
        /// </summary>
        public async Task RollbackAsync()
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("No hay una transacción activa para revertir.");
            }

            try
            {
                await _transaction.RollbackAsync();
            }
            finally
            {
                await DisposeTransactionAsync();
            }
        }

        /// <summary>
        /// Libera los recursos utilizados por la transacción actual.
        /// </summary>
        private async Task DisposeTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        /// <summary>
        /// Libera los recursos del contexto y la transacción si existe.
        /// </summary>
        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
