namespace GestionActivos.Application.ActivoApplication.DTOs
{
    /// <summary>
    /// DTO que representa un activo con su historial completo de reubicaciones.
    /// </summary>
    public class ActivoConHistorialDto
    {
        // Información del activo
        public Guid IdActivo { get; set; }
        public string? Nombre { get; set; }
        public string? Marca { get; set; }
        public string? Modelo { get; set; }
        public string? Etiqueta { get; set; }
        public string? NumeroSerie { get; set; }
        public string? Categoria { get; set; }
        public string Estado { get; set; } = string.Empty;
        public DateTime? FechaAdquisicion { get; set; }
        public string? ImagenUrl { get; set; }
        
        // Responsable actual
        public Guid ResponsableActualId { get; set; }
        public string ResponsableActual { get; set; } = string.Empty;
        public string? CentroCostoActual { get; set; }
        
        // Historial de reubicaciones ordenado por fecha descendente
        public List<ReubicacionDto> HistorialReubicaciones { get; set; } = new();
    }

    /// <summary>
    /// DTO que representa una reubicación en el historial.
    /// </summary>
    public class ReubicacionDto
    {
        public int IdReubicacion { get; set; }
        public DateTime Fecha { get; set; }
        public string? Motivo { get; set; }
        
        // Usuario anterior
        public Guid IdUsuarioAnterior { get; set; }
        public string UsuarioAnterior { get; set; } = string.Empty;
        public string? CentroCostoAnterior { get; set; }
        
        // Usuario nuevo
        public Guid IdUsuarioNuevo { get; set; }
        public string UsuarioNuevo { get; set; } = string.Empty;
        public string? CentroCostoNuevo { get; set; }
    }
}
