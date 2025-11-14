namespace GestionActivos.Application.SolicitudApplication.DTOs
{
    public class SolicitudDto
    {
        public Guid IdSolicitud { get; set; }
        public Guid IdEmisor { get; set; }
        public string NombreEmisor { get; set; } = string.Empty;
        public Guid IdReceptor { get; set; }
        public string NombreReceptor { get; set; } = string.Empty;
        public Guid IdActivo { get; set; }
        public string? EtiquetaActivo { get; set; }
        public string? DescripcionActivo { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string? Mensaje { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; } = string.Empty;
    }
}
