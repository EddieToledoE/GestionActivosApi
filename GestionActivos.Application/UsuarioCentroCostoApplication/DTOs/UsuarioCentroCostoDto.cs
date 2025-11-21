namespace GestionActivos.Application.UsuarioCentroCostoApplication.DTOs
{
    /// <summary>
    /// DTO que representa un centro de costo asignado a un usuario.
    /// </summary>
    public class UsuarioCentroCostoDto
    {
        public Guid IdUsuario { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public int IdCentroCosto { get; set; }
        public string? RazonSocial { get; set; }
        public string? Ubicacion { get; set; }
        public string? Area { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public bool Activo { get; set; }
    }
}
