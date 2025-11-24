using GestionActivos.Application.ActivoApplication.DTOs;
using MediatR;

namespace GestionActivos.Application.ActivoApplication.Queries
{
    /// <summary>
    /// Query para obtener activos agrupados según el contexto del usuario:
    /// - Activos propios
    /// - Activos por centro de costo (si tiene permiso de visualización externa)
    /// </summary>
    public record GetActivosAgrupadosQuery(
        Guid IdUsuario,
        List<int> IdsCentrosCosto
    ) : IRequest<ActivosAgrupadosResponseDto>;
}
