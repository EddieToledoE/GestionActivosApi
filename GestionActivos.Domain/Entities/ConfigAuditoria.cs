namespace GestionActivos.Domain.Entities
{
    public class ConfigAuditoria
    {
        public int IdConfig { get; set; }

        // 🔹 Periodicidad (Mensual, Bimestral, Quincenal, etc.)
        public string Periodo { get; set; } = "Mensual";

        public bool Activa { get; set; } = true;

        // 🔗 Relaciones
        public int IdCentroCosto { get; set; }
        public CentroCosto CentroCosto { get; set; } = null!;

        public Guid? IdResponsable { get; set; }
        public Usuario? Responsable { get; set; }
    }
}
