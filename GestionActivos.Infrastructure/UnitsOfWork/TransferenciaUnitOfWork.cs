using GestionActivos.Domain.Interfaces;
using GestionActivos.Domain.Interfaces.UnitsOfWork;
using GestionActivos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Storage;

namespace GestionActivos.Infrastructure.UnitsOfWork
{
    /// <summary>
    /// Implementación del UoW para el contexto de Transferencias directas por Auditor.
    /// </summary>
    public class TransferenciaUnitOfWork : ITransferenciaUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _transaction;

        public TransferenciaUnitOfWork(
            ApplicationDbContext context,
            IActivoRepository activoRepository,
            IUsuarioRepository usuarioRepository,
            IReubicacionRepository reubicacionRepository,
            INotificacionRepository notificacionRepository)
        {
            _context = context;
            Activos = activoRepository;
            Usuarios = usuarioRepository;
            Reubicaciones = reubicacionRepository;
            Notificaciones = notificacionRepository;
        }

        public IActivoRepository Activos { get; }
        public IUsuarioRepository Usuarios { get; }
        public IReubicacionRepository Reubicaciones { get; }
        public INotificacionRepository Notificaciones { get; }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            if (_transaction != null)
            {
                throw new InvalidOperationException("Ya existe una transacción activa.");
            }

            _transaction = await _context.Database.BeginTransactionAsync();
        }

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

        private async Task DisposeTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
