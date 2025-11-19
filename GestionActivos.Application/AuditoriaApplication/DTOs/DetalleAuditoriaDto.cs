namespace GestionActivos.Application.AuditoriaApplication.DTOs
{
    /// <summary>
    /// DTO que representa el detalle de un activo auditado.
    /// </summary>
    public class DetalleAuditoriaDto
    {
        public int IdDetalle { get; set; }
        public Guid IdActivo { get; set; }
        public string? EtiquetaActivo { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string? Comentarios { get; set; }
    }
}
