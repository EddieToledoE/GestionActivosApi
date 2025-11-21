namespace GestionActivos.Application.UsuarioCentroCostoApplication.DTOs
{
    /// <summary>
    /// DTO para asignar un centro de costo a un usuario.
    /// </summary>
    public class AsignarCentroCostoDto
    {
        public Guid IdUsuario { get; set; }
        public int IdCentroCosto { get; set; }
        public DateTime? FechaInicio { get; set; }
    }
}
