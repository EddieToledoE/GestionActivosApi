using GestionActivos.Domain.Entities;

namespace GestionActivos.Domain.Interfaces
{
    public interface IReubicacionRepository
    {
        Task<Reubicacion?> GetByIdAsync(int id);
        Task<IEnumerable<Reubicacion>> GetAllAsync();
        Task<IEnumerable<Reubicacion>> GetByActivoIdAsync(int activoId);
        Task<IEnumerable<Reubicacion>> GetByUsuarioAsync(int usuarioId);
        Task AddAsync(Reubicacion reubicacion);
        Task UpdateAsync(Reubicacion reubicacion);
        Task DeleteAsync(int id);
    }
}
