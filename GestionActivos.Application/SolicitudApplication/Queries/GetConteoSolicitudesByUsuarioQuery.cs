using GestionActivos.Application.SolicitudApplication.DTOs;
using MediatR;

namespace GestionActivos.Application.SolicitudApplication.Queries
{
    /// <summary>
    /// Query para obtener el conteo de solicitudes pendientes de un usuario.
    /// Incluye solicitudes donde el usuario es emisor o receptor.
    /// </summary>
    public record GetConteoSolicitudesByUsuarioQuery(Guid IdUsuario) : IRequest<ConteoSolicitudesDto>;
}
