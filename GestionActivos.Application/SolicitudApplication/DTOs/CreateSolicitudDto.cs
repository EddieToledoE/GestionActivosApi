namespace GestionActivos.Application.SolicitudApplication.DTOs
{
    public class CreateSolicitudDto
    {
        public int IdEmisor { get; set; }
        public int IdReceptor { get; set; }
        public int IdActivo { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string? Mensaje { get; set; }
    }
}
