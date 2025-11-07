namespace GestionActivos.Domain.Entities
{
    public class Auditoria
    {
        public int IdAuditoria { get; set; }
        public string Tipo { get; set; } = string.Empty; // Auto / Externa
        public int IdAuditor { get; set; }
        public int IdUsuarioAuditado { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string? Observaciones { get; set; }

        public Usuario Auditor { get; set; } = null!;
        public Usuario UsuarioAuditado { get; set; } = null!;
        public ICollection<DetalleAuditoria> Detalles { get; set; } = new List<DetalleAuditoria>();
    }
}
