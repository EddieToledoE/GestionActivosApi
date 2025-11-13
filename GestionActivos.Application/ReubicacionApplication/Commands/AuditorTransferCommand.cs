using MediatR;

namespace GestionActivos.Application.ReubicacionApplication.Commands
{
    /// <summary>
    /// Comando para que un auditor transfiera directamente un activo de un usuario a otro.
    /// No requiere aprobación, es una transferencia administrativa directa.
    /// </summary>
    public record AuditorTransferCommand : IRequest<bool>
    {
        /// <summary>
        /// ID del auditor que realiza la transferencia.
        /// </summary>
        public int IdAuditor { get; init; }

        /// <summary>
        /// ID del activo que se va a transferir.
        /// </summary>
        public int IdActivo { get; init; }

        /// <summary>
        /// ID del usuario destino que recibirá el activo.
        /// </summary>
        public int IdUsuarioDestino { get; init; }

        /// <summary>
        /// Motivo de la transferencia.
        /// </summary>
        public string Motivo { get; init; } = string.Empty;
    }
}
