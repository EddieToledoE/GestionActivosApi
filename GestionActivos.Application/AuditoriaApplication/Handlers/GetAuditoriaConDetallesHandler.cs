using GestionActivos.Application.AuditoriaApplication.DTOs;
using GestionActivos.Application.AuditoriaApplication.Queries;
using GestionActivos.Domain.Exceptions;
using GestionActivos.Domain.Interfaces;
using MediatR;

namespace GestionActivos.Application.AuditoriaApplication.Handlers
{
    /// <summary>
    /// Handler para obtener una auditoría completa con sus detalles.
    /// 
    /// Flujo:
    /// 1. Obtiene la auditoría por ID
    /// 2. Valida que la auditoría exista
    /// 3. Proyecta a DTO con información completa de la auditoría y sus detalles
    /// 4. Incluye información de auditor, usuario auditado y centro de costo
    /// 5. Incluye detalles de cada activo auditado
    /// </summary>
    public class GetAuditoriaConDetallesHandler : IRequestHandler<GetAuditoriaConDetallesQuery, AuditoriaConDetallesDto>
    {
        private readonly IAuditoriaRepository _auditoriaRepository;

        public GetAuditoriaConDetallesHandler(IAuditoriaRepository auditoriaRepository)
        {
            _auditoriaRepository = auditoriaRepository;
        }

        public async Task<AuditoriaConDetallesDto> Handle(
            GetAuditoriaConDetallesQuery request,
            CancellationToken cancellationToken)
        {
            // 1. Obtener la auditoría con sus detalles
            var auditoria = await _auditoriaRepository.GetByIdAsync(request.IdAuditoria);
            
            if (auditoria == null)
            {
                throw new NotFoundException($"No se encontró la auditoría con ID {request.IdAuditoria}.");
            }

            // 2. Construir clave del centro de costo con formato: RazonSocial_Ubicacion_Area
            string nombreCentroCosto;
            if (auditoria.CentroCosto != null)
            {
                var razonSocial = !string.IsNullOrEmpty(auditoria.CentroCosto.RazonSocial) 
                    ? auditoria.CentroCosto.RazonSocial : "SinRazonSocial";
                var ubicacion = !string.IsNullOrEmpty(auditoria.CentroCosto.Ubicacion) 
                    ? auditoria.CentroCosto.Ubicacion : "SinUbicacion";
                var area = !string.IsNullOrEmpty(auditoria.CentroCosto.Area) 
                    ? auditoria.CentroCosto.Area : "SinArea";
                
                nombreCentroCosto = $"{razonSocial}_{ubicacion}_{area}";
            }
            else
            {
                nombreCentroCosto = $"CentroCosto_{auditoria.IdCentroCosto}";
            }

            // 3. Construir DTO con información completa
            var response = new AuditoriaConDetallesDto
            {
                // Información de la auditoría
                IdAuditoria = auditoria.IdAuditoria,
                Tipo = auditoria.Tipo,
                Fecha = auditoria.Fecha,
                Observaciones = auditoria.Observaciones,
                
                // Auditor
                IdAuditor = auditoria.IdAuditor,
                Auditor = $"{auditoria.Auditor.Nombres} {auditoria.Auditor.ApellidoPaterno} {auditoria.Auditor.ApellidoMaterno}".Trim(),
                
                // Usuario auditado
                IdUsuarioAuditado = auditoria.IdUsuarioAuditado,
                UsuarioAuditado = $"{auditoria.UsuarioAuditado.Nombres} {auditoria.UsuarioAuditado.ApellidoPaterno} {auditoria.UsuarioAuditado.ApellidoMaterno}".Trim(),
                
                // Centro de costo
                IdCentroCosto = auditoria.IdCentroCosto,
                CentroCosto = nombreCentroCosto,
                
                // Detalles de activos auditados
                Detalles = auditoria.Detalles.Select(d => new DetalleAuditoriaResumenDto
                {
                    IdDetalle = d.IdDetalle,
                    
                    // Información del activo
                    IdActivo = d.IdActivo,
                    NombreActivo = d.Activo.Descripcion ?? "Sin nombre",
                    Etiqueta = d.Activo.Etiqueta,
                    Marca = d.Activo.Marca,
                    Modelo = d.Activo.Modelo,
                    Categoria = d.Activo.CategoriaNavigation?.Nombre,
                    
                    // Resultado de la auditoría
                    Estado = d.Estado,
                    Comentarios = d.Comentarios
                }).ToList()
            };

            return response;
        }
    }
}
