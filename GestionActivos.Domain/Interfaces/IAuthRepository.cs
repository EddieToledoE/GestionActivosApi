using System.Collections.Generic;
using System.Threading.Tasks;
using GestionActivos.Domain.Entities;

namespace GestionActivos.Domain.Interfaces
{
    public interface IAuthRepository
    {
        /// <summary>
        /// Valida las credenciales del usuario.
        /// </summary>
        Task<Usuario?> LoginAsync(string correo, string contrasena);
    }
}
