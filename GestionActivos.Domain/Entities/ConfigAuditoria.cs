namespace GestionActivos.Domain.Entities
{
    public class ConfigAuditoria
    {
        public int IdConfig { get; set; }
        public string? Periodo { get; set; } // Mensual, Bimestral, etc.
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public int IdResponsable { get; set; }

        public Usuario Responsable { get; set; } = null!;
    }
}
