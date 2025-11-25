using GestionActivos.Application.AuditoriaApplication.DTOs;
using MediatR;

namespace GestionActivos.Application.AuditoriaApplication.Queries
{
    /// <summary>
    /// Query para obtener una auditoría completa con sus detalles.
    /// </summary>
    public record GetAuditoriaConDetallesQuery(Guid IdAuditoria) : IRequest<AuditoriaConDetallesDto>;
}
