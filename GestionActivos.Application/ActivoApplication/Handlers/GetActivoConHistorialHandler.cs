using GestionActivos.Application.ActivoApplication.DTOs;
using GestionActivos.Application.ActivoApplication.Queries;
using GestionActivos.Domain.Exceptions;
using GestionActivos.Domain.Interfaces;
using MediatR;

namespace GestionActivos.Application.ActivoApplication.Handlers
{
    /// <summary>
    /// Handler para obtener un activo con su historial completo de reubicaciones.
    /// 
    /// Flujo:
    /// 1. Obtiene el activo por ID
    /// 2. Valida que el activo exista
    /// 3. Obtiene el historial de reubicaciones del activo
    /// 4. Proyecta a DTO con información completa del activo y su historial
    /// 5. Ordena reubicaciones por fecha descendente (más reciente primero)
    /// </summary>
    public class GetActivoConHistorialHandler : IRequestHandler<GetActivoConHistorialQuery, ActivoConHistorialDto>
    {
        private readonly IActivoRepository _activoRepository;
        private readonly IReubicacionRepository _reubicacionRepository;

        public GetActivoConHistorialHandler(
            IActivoRepository activoRepository,
            IReubicacionRepository reubicacionRepository)
        {
            _activoRepository = activoRepository;
            _reubicacionRepository = reubicacionRepository;
        }

        public async Task<ActivoConHistorialDto> Handle(
            GetActivoConHistorialQuery request,
            CancellationToken cancellationToken)
        {
            // 1. Obtener el activo
            var activo = await _activoRepository.GetByIdAsync(request.IdActivo);
            
            if (activo == null)
            {
                throw new NotFoundException($"No se encontró el activo con ID {request.IdActivo}.");
            }

            // 2. Obtener historial de reubicaciones
            var reubicaciones = await _reubicacionRepository.GetByActivoIdAsync(request.IdActivo);

            // 3. Construir DTO con información completa
            var response = new ActivoConHistorialDto
            {
                // Información del activo
                IdActivo = activo.IdActivo,
                Nombre = activo.Descripcion,
                Marca = activo.Marca,
                Modelo = activo.Modelo,
                Etiqueta = activo.Etiqueta,
                NumeroSerie = activo.NumeroSerie,
                Categoria = activo.CategoriaNavigation?.Nombre,
                Estado = activo.Estatus,
                FechaAdquisicion = activo.FechaAdquisicion,
                ImagenUrl = activo.ImagenUrl,
                
                // Responsable actual
                ResponsableActualId = activo.ResponsableId,
                ResponsableActual = $"{activo.Responsable.Nombres} {activo.Responsable.ApellidoPaterno} {activo.Responsable.ApellidoMaterno}".Trim(),
                CentroCostoActual = activo.Responsable.CentroCosto != null
                    ? $"{activo.Responsable.CentroCosto.RazonSocial}_{activo.Responsable.CentroCosto.Ubicacion}_{activo.Responsable.CentroCosto.Area}"
                    : null,
                
                // Historial de reubicaciones (ordenado por fecha descendente)
                HistorialReubicaciones = reubicaciones
                    .OrderByDescending(r => r.Fecha)
                    .Select(r => new ReubicacionDto
                    {
                        IdReubicacion = r.IdReubicacion,
                        Fecha = r.Fecha,
                        Motivo = r.Motivo,
                        
                        // Usuario anterior
                        IdUsuarioAnterior = r.IdUsuarioAnterior,
                        UsuarioAnterior = $"{r.UsuarioAnterior.Nombres} {r.UsuarioAnterior.ApellidoPaterno} {r.UsuarioAnterior.ApellidoMaterno}".Trim(),
                        CentroCostoAnterior = r.UsuarioAnterior.CentroCosto != null
                            ? $"{r.UsuarioAnterior.CentroCosto.RazonSocial}_{r.UsuarioAnterior.CentroCosto.Ubicacion}_{r.UsuarioAnterior.CentroCosto.Area}"
                            : null,
                        
                        // Usuario nuevo
                        IdUsuarioNuevo = r.IdUsuarioNuevo,
                        UsuarioNuevo = $"{r.UsuarioNuevo.Nombres} {r.UsuarioNuevo.ApellidoPaterno} {r.UsuarioNuevo.ApellidoMaterno}".Trim(),
                        CentroCostoNuevo = r.UsuarioNuevo.CentroCosto != null
                            ? $"{r.UsuarioNuevo.CentroCosto.RazonSocial}_{r.UsuarioNuevo.CentroCosto.Ubicacion}_{r.UsuarioNuevo.CentroCosto.Area}"
                            : null
                    })
                    .ToList()
            };

            return response;
        }
    }
}
