using Microsoft.AspNetCore.Http;

namespace GestionActivos.Application.ActivoApplication.DTOs
{
    public class CreateActivoDto
    {
        public IFormFile? Imagen { get; set; }
        public int ResponsableId { get; set; }
        public int IdCategoria { get; set; }
        public string? Marca { get; set; }
        public string? Modelo { get; set; }
        public string? Descripcion { get; set; }
        public string? Etiqueta { get; set; }
        public string? NumeroSerie { get; set; }
        public bool Donacion { get; set; } = false;
        public IFormFile? FacturaPDF { get; set; }
        public IFormFile? FacturaXML { get; set; }
        public string? CuentaContable { get; set; }
        public decimal? ValorAdquisicion { get; set; }
        public DateTime? FechaAdquisicion { get; set; }
        public bool PortaEtiqueta { get; set; } = false;
        public bool CuentaContableEtiqueta { get; set; } = false;
    }
}
