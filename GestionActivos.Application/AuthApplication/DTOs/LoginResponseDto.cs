namespace GestionActivos.Application.AuthApplication.DTOs
{
    /// <summary>
    /// DTO completo que se retorna al hacer login exitoso.
    /// Incluye información del usuario, roles, permisos y centros de costo.
    /// </summary>
    public class LoginResponseDto
    {
        public Guid IdUsuario { get; set; }
        public string Nombres { get; set; } = string.Empty;
        public string ApellidoPaterno { get; set; } = string.Empty;
        public string? ApellidoMaterno { get; set; }
        public string? Correo { get; set; }
        public string? ClaveFortia { get; set; }
        
        /// <summary>
        /// Centro de costo principal (legacy).
        /// </summary>
        public int? IdCentroCostoPrincipal { get; set; }
        public string? CentroCostoPrincipal { get; set; }
        
        /// <summary>
        /// Roles asignados al usuario.
        /// </summary>
        public List<RolInfoDto> Roles { get; set; } = new();
        
        /// <summary>
        /// Lista plana de permisos únicos del usuario (agregados de todos sus roles).
        /// Para control de UI/frontend.
        /// </summary>
        public List<string> Permisos { get; set; } = new();
        
        /// <summary>
        /// Centros de costo a los que tiene acceso el usuario.
        /// Para control de peticiones backend (filtrado de datos).
        /// </summary>
        public List<CentroCostoAccessDto> CentrosCostoAcceso { get; set; } = new();
    }

    /// <summary>
    /// Información de un rol del usuario.
    /// </summary>
    public class RolInfoDto
    {
        public int IdRol { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public List<string> Permisos { get; set; } = new();
    }

    /// <summary>
    /// Información de acceso a un centro de costo.
    /// </summary>
    public class CentroCostoAccessDto
    {
        public int IdCentroCosto { get; set; }
        public string? RazonSocial { get; set; }
        public string? Ubicacion { get; set; }
        public string? Area { get; set; }
        public bool Activo { get; set; }
    }
}
