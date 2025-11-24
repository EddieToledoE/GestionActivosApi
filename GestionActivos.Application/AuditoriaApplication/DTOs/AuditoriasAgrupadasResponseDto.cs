namespace GestionActivos.Application.AuditoriaApplication.DTOs
{
    /// <summary>
    /// DTO que representa una auditoría resumida (sin detalles).
    /// </summary>
    public class AuditoriaResumenDto
    {
        public Guid IdAuditoria { get; set; }
        public DateTime Fecha { get; set; }
        public string Tipo { get; set; } = string.Empty; // Auto o Externa
        public string? Observaciones { get; set; }
        
        // Para tipo "Auto"
        public string? Responsable { get; set; }
        
        // Para tipo "Externa"
        public string? Auditor { get; set; }
        public string? UsuarioAuditado { get; set; }
        
        public string Estado { get; set; } = "Completada"; // Por ahora todas son completadas
    }

    /// <summary>
    /// DTO que agrupa auditorías por tipo dentro de un centro de costo.
    /// </summary>
    public class AuditoriasPorTipoDto
    {
        public List<AuditoriaResumenDto> Auto { get; set; } = new();
        public List<AuditoriaResumenDto> Externa { get; set; } = new();
    }

    /// <summary>
    /// DTO de respuesta completa: auditorías agrupadas por centro de costo y tipo.
    /// </summary>
    public class AuditoriasAgrupadasResponseDto
    {
        public Dictionary<string, AuditoriasPorTipoDto> CentrosCosto { get; set; } = new();
    }
}
