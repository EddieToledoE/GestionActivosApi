namespace GestionActivos.Domain.Entities
{
    public class Solicitud
    {
        public int IdSolicitud { get; set; }
        public int IdEmisor { get; set; }
        public int IdReceptor { get; set; }
        public int IdActivo { get; set; }
        public string Tipo { get; set; } = string.Empty; // Transferencia / Baja / Diagnóstico / Auditoría
        public string? Mensaje { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string Estado { get; set; } = "Pendiente"; // Pendiente / Aceptada / Rechazada

        public Usuario Emisor { get; set; } = null!;
        public Usuario Receptor { get; set; } = null!;
        public Activo Activo { get; set; } = null!;
    }
}
