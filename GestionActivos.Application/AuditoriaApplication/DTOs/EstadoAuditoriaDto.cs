namespace GestionActivos.Application.AuditoriaApplication.DTOs
{
    /// <summary>
    /// DTO que representa el estado de auditoría de un usuario.
    /// </summary>
    public class EstadoAuditoriaDto
    {
        /// <summary>
        /// Periodo de auditoría configurado (Mensual, Bimestral, Quincenal, Semanal).
        /// </summary>
        public string Periodo { get; set; } = string.Empty;

        /// <summary>
        /// Indica si el usuario tiene una auditoría pendiente en el periodo actual.
        /// </summary>
        public bool Pendiente { get; set; }

        /// <summary>
        /// Mensaje descriptivo del estado de la auditoría.
        /// </summary>
        public string Mensaje { get; set; } = string.Empty;

        /// <summary>
        /// Fecha de la última auditoría realizada (null si nunca ha tenido).
        /// </summary>
        public DateTime? FechaUltimaAuditoria { get; set; }

        /// <summary>
        /// Nombre del centro de costo al que pertenece el usuario.
        /// </summary>
        public string? CentroCosto { get; set; }
    }
}
