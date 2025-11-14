using GestionActivos.Domain.Entities;

namespace GestionActivos.Domain.Interfaces
{
    public interface ISolicitudRepository
    {
        Task<Solicitud?> GetByIdAsync(Guid id);
        Task<IEnumerable<Solicitud>> GetAllAsync();
        Task<IEnumerable<Solicitud>> GetByEmisorIdAsync(Guid emisorId);
        Task<IEnumerable<Solicitud>> GetByReceptorIdAsync(Guid receptorId);
        Task<bool> ExisteSolicitudPendienteParaActivoAsync(Guid idActivo);
        Task AddAsync(Solicitud solicitud);
        Task UpdateAsync(Solicitud solicitud);
        Task DeleteAsync(Guid id);
    }
}
