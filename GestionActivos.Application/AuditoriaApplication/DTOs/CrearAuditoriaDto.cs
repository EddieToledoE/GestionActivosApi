namespace GestionActivos.Application.AuditoriaApplication.DTOs
{
    /// <summary>
    /// DTO para registrar una nueva auditoría con sus detalles.
    /// El tipo de auditoría (Auto/Externa) se determina automáticamente:
    /// - Auto: cuando IdAuditor == IdUsuarioAuditado
    /// - Externa: cuando son diferentes personas
    /// 
    /// El IdCentroCosto se obtiene automáticamente del usuario auditado al momento de la inserción
    /// para mantener integridad histórica.
    /// </summary>
    public class CrearAuditoriaDto
    {
        public Guid IdAuditor { get; set; }
        public Guid IdUsuarioAuditado { get; set; }
        public string? Observaciones { get; set; }
        public List<CrearDetalleAuditoriaDto> Detalles { get; set; } = new();
    }
}
