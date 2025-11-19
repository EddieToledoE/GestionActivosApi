namespace GestionActivos.Domain.Entities
{
    public class Auditoria
    {
        public Guid IdAuditoria { get; set; }
        public string Tipo { get; set; } = string.Empty; // Auto / Externa
        public Guid IdAuditor { get; set; }
        public Guid IdUsuarioAuditado { get; set; }
        
        /// <summary>
        /// ID del centro de costo donde se realizó la auditoría.
        /// Se guarda para mantener integridad histórica (el usuario puede cambiar de centro).
        /// </summary>
        public int IdCentroCosto { get; set; }
        
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string? Observaciones { get; set; }

        // 🔗 Relaciones
        public Usuario Auditor { get; set; } = null!;
        public Usuario UsuarioAuditado { get; set; } = null!;
        public CentroCosto CentroCosto { get; set; } = null!;
        public ICollection<DetalleAuditoria> Detalles { get; set; } = new List<DetalleAuditoria>();
    }
}
