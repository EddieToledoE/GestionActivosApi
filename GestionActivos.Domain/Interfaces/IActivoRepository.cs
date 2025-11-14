using GestionActivos.Domain.Entities;

namespace GestionActivos.Domain.Interfaces
{
    public interface IActivoRepository
    {
        Task<Activo?> GetByIdAsync(Guid id);
        Task<IEnumerable<Activo>> GetAllAsync();
        Task<IEnumerable<Activo>> GetByResponsableIdAsync(Guid responsableId);
        Task AddAsync(Activo activo);
        Task UpdateAsync(Activo activo);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsByEtiquetaAsync(string etiqueta);
        Task<bool> ExistsByNumeroSerieAsync(string numeroSerie);
    }
}
