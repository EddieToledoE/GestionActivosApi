namespace GestionActivos.Domain.Entities
{
    public class Diagnostico
    {
        public int IdDiagnostico { get; set; }
        public int IdActivo { get; set; }
        public int IdTecnico { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string Tipo { get; set; } = null!;
        public string? Observaciones { get; set; }

        public Activo Activo { get; set; } = null!;
        public Usuario Tecnico { get; set; } = null!;
    }
}
