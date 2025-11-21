namespace GestionActivos.Application.RolApplication.DTOs
{
    /// <summary>
    /// DTO que representa un rol con sus permisos.
    /// </summary>
    public class RolDto
    {
        public int IdRol { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public List<PermisoDto> Permisos { get; set; } = new();
    }

    /// <summary>
    /// DTO que representa un permiso.
    /// </summary>
    public class PermisoDto
    {
        public int IdPermiso { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }
}
