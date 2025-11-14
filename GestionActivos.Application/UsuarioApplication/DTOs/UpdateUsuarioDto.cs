namespace GestionActivos.Application.UsuarioApplication.DTOs
{
    public class UpdateUsuarioDto
    {
        public Guid Id { get; set; }
        public string? Nombres { get; set; }
        public string? ApellidoPaterno { get; set; }
        public string? ApellidoMaterno { get; set; }
        public string? Correo { get; set; }
        public string? Contrasena { get; set; }
    }
}
