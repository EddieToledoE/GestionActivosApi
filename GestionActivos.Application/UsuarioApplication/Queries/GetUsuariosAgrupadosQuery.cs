using GestionActivos.Application.UsuarioApplication.DTOs;
using MediatR;

namespace GestionActivos.Application.UsuarioApplication.Queries
{
    /// <summary>
    /// Query para obtener usuarios agrupados por centro de costo.
    /// Filtrado por centros accesibles al usuario solicitante.
    /// </summary>
    public record GetUsuariosAgrupadosQuery(
        Guid IdUsuarioSolicitante,
        List<int> IdsCentrosCostoAcceso,
        string? SearchTerm = null
    ) : IRequest<UsuariosAgrupadosResponseDto>;
}
