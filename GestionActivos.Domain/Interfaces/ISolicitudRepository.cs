using GestionActivos.Domain.Entities;

namespace GestionActivos.Domain.Interfaces
{
    public interface ISolicitudRepository
    {
        Task<Solicitud?> GetByIdAsync(int id);
        Task<IEnumerable<Solicitud>> GetAllAsync();
        Task<IEnumerable<Solicitud>> GetByEmisorIdAsync(int emisorId);
        Task<IEnumerable<Solicitud>> GetByReceptorIdAsync(int receptorId);
        Task AddAsync(Solicitud solicitud);
        Task UpdateAsync(Solicitud solicitud);
        Task DeleteAsync(int id);
    }
}
