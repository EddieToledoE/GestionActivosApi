namespace GestionActivos.Domain.Entities
{
    public class Activo
    {
        public Guid IdActivo { get; set; } = Guid.NewGuid();
        public string? ImagenUrl { get; set; }
        public Guid ResponsableId { get; set; }
        public int IdCategoria { get; set; }
        public string? Marca { get; set; }
        public string? Modelo { get; set; }
        public string? Descripcion { get; set; }
        public string? Etiqueta { get; set; }
        public string? NumeroSerie { get; set; }
        public bool Donacion { get; set; } = false;
        public string? FacturaPDF { get; set; }
        public string? FacturaXML { get; set; }
        public string? CuentaContable { get; set; }
        public decimal? ValorAdquisicion { get; set; }
        public string Estatus { get; set; } = "Activo";
        public DateTime? FechaAdquisicion { get; set; }
        public DateTime FechaAlta { get; set; } = DateTime.Now;
        public bool PortaEtiqueta { get; set; } = false;
        public bool CuentaContableEtiqueta { get; set; } = false;

        // 🔗 Relaciones
        public Usuario Responsable { get; set; } = null!;
        public Categoria CategoriaNavigation { get; set; } = null!;
        public ICollection<Reubicacion> Reubicaciones { get; set; } = new List<Reubicacion>();
        public ICollection<Diagnostico> Diagnosticos { get; set; } = new List<Diagnostico>();
        public ICollection<DetalleAuditoria> DetallesAuditoria { get; set; } =
            new List<DetalleAuditoria>();
    }
}
