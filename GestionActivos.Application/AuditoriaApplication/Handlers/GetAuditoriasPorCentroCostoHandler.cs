using GestionActivos.Application.AuditoriaApplication.DTOs;
using GestionActivos.Application.AuditoriaApplication.Queries;
using GestionActivos.Domain.Interfaces;
using MediatR;

namespace GestionActivos.Application.AuditoriaApplication.Handlers
{
    /// <summary>
    /// Handler para obtener auditorías filtradas por centro de costo.
    /// 
    /// Flujo:
    /// 1. Consulta auditorías del centro de costo especificado
    /// 2. Incluye relaciones: Auditor, UsuarioAuditado, CentroCosto, Detalles
    /// 3. Mapea las entidades a DTOs
    /// 4. Retorna la lista de auditorías
    /// </summary>
    public class GetAuditoriasPorCentroCostoHandler 
        : IRequestHandler<GetAuditoriasPorCentroCostoQuery, IEnumerable<AuditoriaDto>>
    {
        private readonly IAuditoriaRepository _auditoriaRepository;

        public GetAuditoriasPorCentroCostoHandler(IAuditoriaRepository auditoriaRepository)
        {
            _auditoriaRepository = auditoriaRepository;
        }

        public async Task<IEnumerable<AuditoriaDto>> Handle(
            GetAuditoriasPorCentroCostoQuery request,
            CancellationToken cancellationToken)
        {
            // Obtener auditorías del centro de costo (el repositorio debe incluir las relaciones)
            var auditorias = await _auditoriaRepository
                .GetAuditoriasPorCentroCostoAsync(request.IdCentroCosto);

            // Mapear a DTOs
            var resultado = auditorias.Select(a => new AuditoriaDto
            {
                IdAuditoria = a.IdAuditoria,
                Tipo = a.Tipo,
                Fecha = a.Fecha,
                Observaciones = a.Observaciones,
                IdAuditor = a.IdAuditor,
                NombreAuditor = $"{a.Auditor.Nombres} {a.Auditor.ApellidoPaterno}".Trim(),
                IdUsuarioAuditado = a.IdUsuarioAuditado,
                NombreUsuarioAuditado = $"{a.UsuarioAuditado.Nombres} {a.UsuarioAuditado.ApellidoPaterno}".Trim(),
                IdCentroCosto = a.IdCentroCosto,
                CentroCosto = a.CentroCosto?.RazonSocial ?? a.CentroCosto?.Ubicacion,
                Detalles = a.Detalles.Select(d => new DetalleAuditoriaDto
                {
                    IdDetalle = d.IdDetalle,
                    IdActivo = d.IdActivo,
                    EtiquetaActivo = d.Activo?.Etiqueta,
                    Estado = d.Estado,
                    Comentarios = d.Comentarios
                }).ToList()
            }).ToList();

            return resultado;
        }
    }
}
