namespace GestionActivos.Application.UsuarioApplication.DTOs
{
    public class UsuarioCentroCostoDto
    {
        public int IdUsuario { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string? ClaveFortia { get; set; }
        public int? IdCentroCosto { get; set; }
        public string? Ubicacion { get; set; }
        public string? RazonSocial { get; set; }
        public string? Area { get; set; }
    }
}
