namespace GestionActivos.Domain.Entities
{
    /// <summary>
    /// Entidad intermedia que representa la relación muchos-a-muchos entre Usuario y CentroCosto.
    /// Permite que un usuario tenga acceso a múltiples centros de costo (empresas/ubicaciones).
    /// </summary>
    public class UsuarioCentroCosto
    {
        /// <summary>
        /// ID del usuario (parte de la clave compuesta).
        /// </summary>
        public Guid IdUsuario { get; set; }

        /// <summary>
        /// ID del centro de costo (parte de la clave compuesta).
        /// </summary>
        public int IdCentroCosto { get; set; }

        /// <summary>
        /// Fecha de inicio de la asignación del usuario al centro de costo.
        /// </summary>
        public DateTime? FechaInicio { get; set; }

        /// <summary>
        /// Fecha de fin de la asignación (null si está actualmente activo).
        /// </summary>
        public DateTime? FechaFin { get; set; }

        /// <summary>
        /// Indica si la relación está activa.
        /// </summary>
        public bool Activo { get; set; } = true;

        // ?? Relaciones
        /// <summary>
        /// Usuario asociado.
        /// </summary>
        public Usuario Usuario { get; set; } = null!;

        /// <summary>
        /// Centro de costo asociado.
        /// </summary>
        public CentroCosto CentroCosto { get; set; } = null!;
    }
}
