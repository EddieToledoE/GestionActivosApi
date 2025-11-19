namespace GestionActivos.Application.AuditoriaApplication.DTOs
{
    /// <summary>
    /// DTO completo de una auditoría con sus detalles.
    /// </summary>
    public class AuditoriaDto
    {
        public Guid IdAuditoria { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string? Observaciones { get; set; }
        
        // Información del auditor
        public Guid IdAuditor { get; set; }
        public string NombreAuditor { get; set; } = string.Empty;
        
        // Información del usuario auditado
        public Guid IdUsuarioAuditado { get; set; }
        public string NombreUsuarioAuditado { get; set; } = string.Empty;
        
        // Información del centro de costo
        public int IdCentroCosto { get; set; }
        public string? CentroCosto { get; set; }
        
        // Detalles de activos auditados
        public List<DetalleAuditoriaDto> Detalles { get; set; } = new();
    }
}
