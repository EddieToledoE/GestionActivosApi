namespace GestionActivos.Application.SolicitudApplication.DTOs
{
    public class CreateSolicitudDto
    {
        public Guid IdEmisor { get; set; }
        public Guid IdReceptor { get; set; }
        public Guid IdActivo { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string? Mensaje { get; set; }
    }
}
