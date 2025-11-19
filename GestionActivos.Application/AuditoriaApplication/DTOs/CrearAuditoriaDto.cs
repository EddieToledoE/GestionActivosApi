namespace GestionActivos.Application.AuditoriaApplication.DTOs
{
    /// <summary>
    /// DTO para registrar una nueva auditoría con sus detalles.
    /// El tipo de auditoría (Auto/Externa) se determina automáticamente:
    /// - Auto: cuando IdAuditor == IdUsuarioAuditado
    /// - Externa: cuando son diferentes personas
    /// </summary>
    public class CrearAuditoriaDto
    {
        public Guid IdAuditor { get; set; }
        public Guid IdUsuarioAuditado { get; set; }
        public int IdCentroCosto { get; set; }
        public string? Observaciones { get; set; }
        public List<CrearDetalleAuditoriaDto> Detalles { get; set; } = new();
    }
}
