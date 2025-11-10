using System.Collections.Generic;
using System.Threading.Tasks;
using GestionActivos.Domain.Entities;

namespace GestionActivos.Domain.Interfaces
{
    public interface ICategoriaRepository
  {
        Task<Categoria?> GetByIdAsync(int id);
        Task<IEnumerable<Categoria>> GetAllAsync();
        Task AddAsync(Categoria categoria);
        Task UpdateAsync(Categoria categoria);
      Task DeleteAsync(int id);
        Task<bool> ExistsByNombreAsync(string nombre);
    }
}
