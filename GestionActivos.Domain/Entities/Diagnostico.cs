namespace GestionActivos.Domain.Entities
{
    public class Diagnostico
    {
        public int IdDiagnostico { get; set; }
        public int IdActivo { get; set; }
        public int IdTecnico { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string? Pieza { get; set; }
        public decimal? ValorAdquisicion { get; set; }
        public bool TramiteGarantia { get; set; } = false;
        public bool SugerirBaja { get; set; } = false;
        public string? Motivo { get; set; }
        public string? Observaciones { get; set; }

        public Activo Activo { get; set; } = null!;
        public Usuario Tecnico { get; set; } = null!;
    }
}
