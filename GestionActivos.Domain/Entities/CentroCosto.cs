namespace GestionActivos.Domain.Entities
{
    public class CentroCosto
    {
        public int IdCentroCosto { get; set; }
        public string? Ubicacion { get; set; }
        public string? RazonSocial { get; set; }
        public string? Area { get; set; }
        public bool Activo { get; set; } = true;
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        // 🔗 Relaciones
        public ICollection<Usuario>? Usuarios { get; set; }
        public ICollection<ConfigAuditoria>? ConfiguracionesAuditoria { get; set; }
    }
}
