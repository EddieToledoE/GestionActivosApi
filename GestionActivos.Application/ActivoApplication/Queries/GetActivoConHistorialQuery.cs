using GestionActivos.Application.ActivoApplication.DTOs;
using MediatR;

namespace GestionActivos.Application.ActivoApplication.Queries
{
    /// <summary>
    /// Query para obtener un activo con su historial completo de reubicaciones.
    /// </summary>
    public record GetActivoConHistorialQuery(Guid IdActivo) : IRequest<ActivoConHistorialDto>;
}
