using GestionActivos.Domain.Entities;

namespace GestionActivos.Domain.Interfaces
{
    /// <summary>
    /// Repositorio para gestionar la relación Usuario-Rol.
    /// </summary>
    public interface IUsuarioRolRepository
    {
        Task<IEnumerable<UsuarioRol>> GetRolesByUsuarioIdAsync(Guid idUsuario);
        Task<bool> ExistsAsync(Guid idUsuario, int idRol);
        Task AddAsync(UsuarioRol usuarioRol);
        Task RemoveAsync(Guid idUsuario, int idRol);
    }
}
