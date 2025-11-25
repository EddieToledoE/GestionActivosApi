namespace GestionActivos.Application.AuditoriaApplication.DTOs
{
    /// <summary>
    /// DTO que representa una auditoría completa con sus detalles.
    /// </summary>
    public class AuditoriaConDetallesDto
    {
        // Información de la auditoría
        public Guid IdAuditoria { get; set; }
        public string Tipo { get; set; } = string.Empty; // Auto o Externa
        public DateTime Fecha { get; set; }
        public string? Observaciones { get; set; }
        
        // Auditor
        public Guid IdAuditor { get; set; }
        public string Auditor { get; set; } = string.Empty;
        
        // Usuario auditado
        public Guid IdUsuarioAuditado { get; set; }
        public string UsuarioAuditado { get; set; } = string.Empty;
        
        // Centro de costo (histórico)
        public int IdCentroCosto { get; set; }
        public string CentroCosto { get; set; } = string.Empty; // Formato: RazonSocial_Ubicacion_Area
        
        // Detalles de activos auditados
        public List<DetalleAuditoriaResumenDto> Detalles { get; set; } = new();
    }

    /// <summary>
    /// DTO que representa un detalle de auditoría.
    /// </summary>
    public class DetalleAuditoriaResumenDto
    {
        public int IdDetalle { get; set; }
        
        // Información del activo
        public Guid IdActivo { get; set; }
        public string NombreActivo { get; set; } = string.Empty;
        public string? Etiqueta { get; set; }
        public string? Marca { get; set; }
        public string? Modelo { get; set; }
        public string? Categoria { get; set; }
        
        // Resultado de la auditoría
        public string Estado { get; set; } = string.Empty; // Conforme, No Conforme, Observado, etc.
        public string? Comentarios { get; set; }
    }
}
