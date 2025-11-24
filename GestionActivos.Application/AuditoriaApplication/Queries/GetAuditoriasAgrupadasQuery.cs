using GestionActivos.Application.AuditoriaApplication.DTOs;
using MediatR;

namespace GestionActivos.Application.AuditoriaApplication.Queries
{
    /// <summary>
    /// Query para obtener auditorías agrupadas por centro de costo y tipo.
    /// Requiere que el usuario tenga el permiso "Auditoria_Ver_Externos".
    /// </summary>
    public record GetAuditoriasAgrupadasQuery(
        Guid IdUsuario,
        List<int> IdsCentrosCostoAcceso
    ) : IRequest<AuditoriasAgrupadasResponseDto>;
}
