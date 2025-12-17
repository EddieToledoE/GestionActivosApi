namespace GestionActivos.Application.SolicitudApplication.DTOs
{
    /// <summary>
    /// DTO que representa el conteo simple de solicitudes pendientes de un usuario.
    /// </summary>
    public class ConteoSolicitudesDto
    {
        /// <summary>
        /// Total de solicitudes pendientes (entrantes + salientes).
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// Desglose individual de solicitudes por dirección.
        /// </summary>
        public ConteoSolicitudesIndividualDto Individual { get; set; } = new();
    }

    /// <summary>
    /// Desglose individual de solicitudes por dirección.
    /// </summary>
    public class ConteoSolicitudesIndividualDto
    {
        /// <summary>
        /// Número de solicitudes entrantes pendientes (usuario como receptor).
        /// </summary>
        public int Entrantes { get; set; }

        /// <summary>
        /// Número de solicitudes salientes pendientes (usuario como emisor).
        /// </summary>
        public int Salientes { get; set; }
    }
}
