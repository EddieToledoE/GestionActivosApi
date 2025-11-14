using GestionActivos.Domain.Entities;

namespace GestionActivos.Domain.Interfaces
{
    public interface IReubicacionRepository
    {
        Task<Reubicacion?> GetByIdAsync(int id);
        Task<IEnumerable<Reubicacion>> GetAllAsync();
        Task<IEnumerable<Reubicacion>> GetByActivoIdAsync(Guid activoId);
        Task<IEnumerable<Reubicacion>> GetByUsuarioAsync(Guid usuarioId);
        Task AddAsync(Reubicacion reubicacion);
        Task UpdateAsync(Reubicacion reubicacion);
        Task DeleteAsync(int id);
    }
}
