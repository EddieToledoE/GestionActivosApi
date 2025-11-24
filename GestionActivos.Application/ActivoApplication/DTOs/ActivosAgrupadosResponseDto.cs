namespace GestionActivos.Application.ActivoApplication.DTOs
{
    /// <summary>
    /// DTO para activo con información resumida.
    /// </summary>
    public class ActivoResumenDto
    {
        public Guid IdActivo { get; set; }
        public string? Nombre { get; set; }
        public string? Categoria { get; set; }
        public string Responsable { get; set; } = string.Empty;
        public string? CentroCosto { get; set; }
        public string Estado { get; set; } = string.Empty;
        public DateTime? FechaAdquisicion { get; set; }
        public string? ImagenUrl { get; set; }
        public string? Marca { get; set; }
        public string? Modelo { get; set; }
        public string? Etiqueta { get; set; }
    }

    /// <summary>
    /// DTO de respuesta agrupada de activos por contexto de usuario.
    /// </summary>
    public class ActivosAgrupadosResponseDto
    {
        /// <summary>
        /// Activos que pertenecen directamente al usuario autenticado.
        /// </summary>
        public List<ActivoResumenDto> Activos_Propios { get; set; } = new();

        /// <summary>
        /// Activos agrupados por centro de costo (solo si tiene permiso de visualización externa).
        /// Estructura dinámica: cada centro de costo es una clave con su lista de activos.
        /// </summary>
        public Dictionary<string, List<ActivoResumenDto>> CentrosCosto { get; set; } = new();
    }
}
