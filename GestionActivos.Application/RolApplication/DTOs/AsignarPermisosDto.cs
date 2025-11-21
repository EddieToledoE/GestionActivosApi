namespace GestionActivos.Application.RolApplication.DTOs
{
    /// <summary>
    /// DTO para asignar permisos a un rol.
    /// </summary>
    public class AsignarPermisosDto
    {
        public int IdRol { get; set; }
        public List<int> IdsPermisos { get; set; } = new();
    }
}
