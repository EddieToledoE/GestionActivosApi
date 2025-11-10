namespace GestionActivos.Application.UsuarioApplication.DTOs
{
    public class UsuarioDto
    {
        public int IdUsuario { get; set; }
        public string Nombres { get; set; } = string.Empty;
        public string ApellidoPaterno { get; set; } = string.Empty;
        public string? ApellidoMaterno { get; set; }

        public string? ClaveFortia { get; set; }
        public string? Correo { get; set; }
        public bool Activo { get; set; }
    }
}
