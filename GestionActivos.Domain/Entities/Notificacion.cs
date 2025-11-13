namespace GestionActivos.Domain.Entities
{
    public class Notificacion
    {
        public int IdNotificacion { get; set; }

        public int IdUsuarioDestino { get; set; }

        // Puede ser "Sistema", "Auditoría", "Administrador", etc.
        public string? Origen { get; set; }

        public string Tipo { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public DateTime Fecha { get; set; } = DateTime.Now;
        public bool Leida { get; set; } = false;

        // 🔗 Relación con el usuario destino
        public Usuario? UsuarioDestino { get; set; }
    }
}
