using GestionActivos.Application.AuditoriaApplication.DTOs;
using MediatR;

namespace GestionActivos.Application.AuditoriaApplication.Commands
{
    /// <summary>
    /// Comando para registrar una nueva auditoría con sus detalles.
    /// </summary>
    public record CrearAuditoriaCommand(CrearAuditoriaDto Auditoria) : IRequest<Guid>;
}
