namespace GestionActivos.Application.ActivoApplication.DTOs
{
    public class ActivoDto
    {
        public int IdActivo { get; set; }
        public string? ImagenUrl { get; set; }
        public int ResponsableId { get; set; }
        public int IdCategoria { get; set; }
        public string? Marca { get; set; }
        public string? Modelo { get; set; }
        public string? Descripcion { get; set; }
     public string? Etiqueta { get; set; }
        public string? NumeroSerie { get; set; }
        public bool Donacion { get; set; }
        public string? Factura { get; set; }
        public decimal? ValorAdquisicion { get; set; }
  public string Estatus { get; set; } = "Activo";
        public DateTime? FechaAdquisicion { get; set; }
    public DateTime FechaAlta { get; set; }
        public bool PortaEtiqueta { get; set; }
    }
}
