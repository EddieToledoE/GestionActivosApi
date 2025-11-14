namespace GestionActivos.Domain.Entities
{
    public class DetalleAuditoria
    {
        public int IdDetalle { get; set; }
        public Guid IdAuditoria { get; set; }
        public Guid IdActivo { get; set; }
        public string Estado { get; set; } = string.Empty; // Encontrado / Faltante / Dañado
        public string? Comentarios { get; set; }

        public Auditoria Auditoria { get; set; } = null!;
        public Activo Activo { get; set; } = null!;
    }
}
