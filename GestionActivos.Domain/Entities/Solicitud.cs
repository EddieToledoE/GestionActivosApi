namespace GestionActivos.Domain.Entities
{
    public class Solicitud
    {
        public Guid IdSolicitud { get; set; } = Guid.NewGuid();
        public Guid IdEmisor { get; set; }
        public Guid IdReceptor { get; set; }
        public Guid IdActivo { get; set; }
        public string Tipo { get; set; } = string.Empty; // Transferencia / Baja / Diagnóstico / Auditoría
        public string? Mensaje { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string Estado { get; set; } = "Pendiente"; // Pendiente / Aceptada / Rechazada

        public Usuario Emisor { get; set; } = null!;
        public Usuario Receptor { get; set; } = null!;
        public Activo Activo { get; set; } = null!;
    }
}
