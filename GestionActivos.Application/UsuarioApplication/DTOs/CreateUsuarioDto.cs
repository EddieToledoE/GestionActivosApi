namespace GestionActivos.Application.UsuarioApplication.DTOs
{
    public class CreateUsuarioDto
    {
        public string Nombres { get; set; } = string.Empty;
        public string ApellidoPaterno { get; set; } = string.Empty;

        public string? ClaveFortia { get; set; }
        public string? ApellidoMaterno { get; set; }
        public string? Correo { get; set; }
        public string Contrasena { get; set; } = string.Empty;
    }
}
