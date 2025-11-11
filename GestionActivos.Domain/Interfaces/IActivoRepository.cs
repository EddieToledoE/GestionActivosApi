using GestionActivos.Domain.Entities;

namespace GestionActivos.Domain.Interfaces
{
    public interface IActivoRepository
    {
        Task<Activo?> GetByIdAsync(int id);
        Task<IEnumerable<Activo>> GetAllAsync();
        Task<IEnumerable<Activo>> GetByResponsableIdAsync(int responsableId);
        Task AddAsync(Activo activo);
        Task UpdateAsync(Activo activo);
        Task DeleteAsync(int id);
        Task<bool> ExistsByEtiquetaAsync(string etiqueta);
        Task<bool> ExistsByNumeroSerieAsync(string numeroSerie);
    }
}
