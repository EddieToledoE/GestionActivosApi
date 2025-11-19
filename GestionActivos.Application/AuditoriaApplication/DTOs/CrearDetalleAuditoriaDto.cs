namespace GestionActivos.Application.AuditoriaApplication.DTOs
{
    /// <summary>
    /// DTO para crear un detalle de auditoría.
    /// </summary>
    public class CrearDetalleAuditoriaDto
    {
        public Guid IdActivo { get; set; }
        public string Estado { get; set; } = string.Empty; // Encontrado, Faltante, Dañado, etc.
        public string? Comentarios { get; set; }
    }
}
