namespace GestionActivos.Application.UsuarioApplication.DTOs
{
    /// <summary>
    /// DTO para asignar un rol a un usuario.
    /// </summary>
    public class AsignarRolDto
    {
        public Guid IdUsuario { get; set; }
        public int IdRol { get; set; }
    }
}
