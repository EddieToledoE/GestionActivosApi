namespace GestionActivos.Application.SolicitudApplication.DTOs
{
    public class SolicitudDto
    {
        public int IdSolicitud { get; set; }
        public int IdEmisor { get; set; }
        public string NombreEmisor { get; set; } = string.Empty;
        public int IdReceptor { get; set; }
        public string NombreReceptor { get; set; } = string.Empty;
        public int IdActivo { get; set; }
        public string? EtiquetaActivo { get; set; }
        public string? DescripcionActivo { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string? Mensaje { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; } = string.Empty;
    }
}
