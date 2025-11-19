using GestionActivos.Application.AuditoriaApplication.DTOs;
using MediatR;

namespace GestionActivos.Application.AuditoriaApplication.Queries
{
    /// <summary>
    /// Query para obtener todas las auditorías de un centro de costo específico.
    /// </summary>
    public record GetAuditoriasPorCentroCostoQuery(int IdCentroCosto) : IRequest<IEnumerable<AuditoriaDto>>;
}
