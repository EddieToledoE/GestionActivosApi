using GestionActivos.Application.AuditoriaApplication.DTOs;
using MediatR;

namespace GestionActivos.Application.AuditoriaApplication.Queries
{
    /// <summary>
    /// Query para obtener el estado de auditoría de un usuario específico.
    /// </summary>
    public record GetEstadoAuditoriaQuery(Guid IdUsuario) : IRequest<EstadoAuditoriaDto>;
}
