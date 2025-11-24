namespace GestionActivos.Application.UsuarioApplication.DTOs
{
    /// <summary>
    /// DTO resumido de usuario para listados agrupados.
    /// No incluye información sensible.
    /// </summary>
    public class UsuarioResumenDto
    {
        public Guid IdUsuario { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string? ClaveFortia { get; set; }
        public string? Correo { get; set; }
    }

    /// <summary>
    /// DTO de respuesta: usuarios agrupados por centro de costo.
    /// </summary>
    public class UsuariosAgrupadosResponseDto
    {
        public Dictionary<string, List<UsuarioResumenDto>> CentrosCosto { get; set; } = new();
    }
}
