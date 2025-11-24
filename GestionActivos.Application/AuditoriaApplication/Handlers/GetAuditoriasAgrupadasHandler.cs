using GestionActivos.Application.AuditoriaApplication.DTOs;
using GestionActivos.Application.AuditoriaApplication.Queries;
using GestionActivos.Domain.Interfaces;
using MediatR;

namespace GestionActivos.Application.AuditoriaApplication.Handlers
{
    /// <summary>
    /// Handler para obtener auditorías agrupadas por centro de costo y tipo.
    /// 
    /// Flujo:
    /// 1. Obtiene todas las auditorías de los centros de costo accesibles
    /// 2. Agrupa por centro de costo
    /// 3. Dentro de cada centro, agrupa por tipo (Auto/Externa)
    /// 4. Ordena por fecha descendente
    /// 5. Proyecta a DTOs sin incluir detalles
    /// </summary>
    public class GetAuditoriasAgrupadasHandler : IRequestHandler<GetAuditoriasAgrupadasQuery, AuditoriasAgrupadasResponseDto>
    {
        private readonly IAuditoriaRepository _auditoriaRepository;

        public GetAuditoriasAgrupadasHandler(IAuditoriaRepository auditoriaRepository)
        {
            _auditoriaRepository = auditoriaRepository;
        }

        public async Task<AuditoriasAgrupadasResponseDto> Handle(
            GetAuditoriasAgrupadasQuery request,
            CancellationToken cancellationToken)
        {
            var response = new AuditoriasAgrupadasResponseDto();

            // Si no tiene acceso a ningún centro, retornar vacío
            if (request.IdsCentrosCostoAcceso == null || !request.IdsCentrosCostoAcceso.Any())
            {
                return response;
            }

            // Obtener todas las auditorías de los centros accesibles
            var auditorias = await _auditoriaRepository.GetAuditoriasByCentrosCostoAsync(request.IdsCentrosCostoAcceso);

            // Agrupar por centro de costo
            var auditoriasPorCentro = auditorias
                .GroupBy(a => a.IdCentroCosto);

            foreach (var grupoCentro in auditoriasPorCentro)
            {
                var idCentroCosto = grupoCentro.Key;
                var nombreCentro = $"CentroCosto_{idCentroCosto}";

                // Obtener nombre real del centro de costo si está disponible
                var primerAuditoria = grupoCentro.FirstOrDefault();
                if (primerAuditoria?.CentroCosto != null)
                {
                    nombreCentro = !string.IsNullOrEmpty(primerAuditoria.CentroCosto.RazonSocial)
                        ? primerAuditoria.CentroCosto.RazonSocial
                        : nombreCentro;
                }

                var auditoriasPorTipo = new AuditoriasPorTipoDto();

                // Agrupar por tipo (Auto/Externa)
                var auditoriasAuto = grupoCentro
                    .Where(a => a.Tipo == "Auto")
                    .OrderByDescending(a => a.Fecha)
                    .Select(a => new AuditoriaResumenDto
                    {
                        IdAuditoria = a.IdAuditoria,
                        Fecha = a.Fecha,
                        Tipo = a.Tipo,
                        Observaciones = a.Observaciones,
                        Responsable = $"{a.UsuarioAuditado.Nombres} {a.UsuarioAuditado.ApellidoPaterno}".Trim(),
                        Estado = "Completada"
                    })
                    .ToList();

                var auditoriasExternas = grupoCentro
                    .Where(a => a.Tipo == "Externa")
                    .OrderByDescending(a => a.Fecha)
                    .Select(a => new AuditoriaResumenDto
                    {
                        IdAuditoria = a.IdAuditoria,
                        Fecha = a.Fecha,
                        Tipo = a.Tipo,
                        Observaciones = a.Observaciones,
                        Auditor = $"{a.Auditor.Nombres} {a.Auditor.ApellidoPaterno}".Trim(),
                        UsuarioAuditado = $"{a.UsuarioAuditado.Nombres} {a.UsuarioAuditado.ApellidoPaterno}".Trim(),
                        Estado = "Completada"
                    })
                    .ToList();

                auditoriasPorTipo.Auto = auditoriasAuto;
                auditoriasPorTipo.Externa = auditoriasExternas;

                response.CentrosCosto[nombreCentro] = auditoriasPorTipo;
            }

            return response;
        }
    }
}
