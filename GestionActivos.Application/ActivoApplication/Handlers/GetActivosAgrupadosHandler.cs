using GestionActivos.Application.ActivoApplication.DTOs;
using GestionActivos.Application.ActivoApplication.Queries;
using GestionActivos.Domain.Interfaces;
using MediatR;

namespace GestionActivos.Application.ActivoApplication.Handlers
{
    /// <summary>
    /// Handler para obtener activos agrupados según el contexto del usuario.
    /// 
    /// Lógica:
    /// 1. Obtiene activos propios del usuario (solo activos)
    /// 2. Si tiene centros de costo adicionales, obtiene activos de esos centros (solo activos)
    /// 3. Agrupa la respuesta de forma dinámica
    /// </summary>
    public class GetActivosAgrupadosHandler : IRequestHandler<GetActivosAgrupadosQuery, ActivosAgrupadosResponseDto>
    {
        private readonly IActivoRepository _activoRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public GetActivosAgrupadosHandler(
            IActivoRepository activoRepository,
            IUsuarioRepository usuarioRepository)
        {
            _activoRepository = activoRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<ActivosAgrupadosResponseDto> Handle(
            GetActivosAgrupadosQuery request,
            CancellationToken cancellationToken)
        {
            var response = new ActivosAgrupadosResponseDto();

            // 1. Obtener activos propios del usuario (solo activos)
            var activosPropios = await _activoRepository.GetByResponsableIdAsync(request.IdUsuario);
            
            response.Activos_Propios = activosPropios
                .Where(a => a.Estatus == "Activo") // ? Filtrar solo activos
                .OrderByDescending(a => a.FechaAdquisicion)
                .Select(a => new ActivoResumenDto
                {
                    IdActivo = a.IdActivo,
                    Nombre = a.Descripcion,
                    Categoria = a.CategoriaNavigation?.Nombre,
                    Responsable = $"{a.Responsable.Nombres} {a.Responsable.ApellidoPaterno}".Trim(),
                    CentroCosto = a.Responsable.CentroCosto != null 
                        ? $"{a.Responsable.CentroCosto.RazonSocial} - {a.Responsable.CentroCosto.Ubicacion}".Trim()
                        : null,
                    Estado = a.Estatus,
                    FechaAdquisicion = a.FechaAdquisicion,
                    ImagenUrl = a.ImagenUrl,
                    Marca = a.Marca,
                    Modelo = a.Modelo,
                    Etiqueta = a.Etiqueta
                })
                .ToList();

            // 2. Si tiene centros de costo adicionales, obtener activos por centro (solo activos)
            if (request.IdsCentrosCosto != null && request.IdsCentrosCosto.Any())
            {
                foreach (var idCentroCosto in request.IdsCentrosCosto)
                {
                    // Obtener activos del centro de costo (excluyendo los propios del usuario)
                    var activosCentro = await _activoRepository.GetActivosByCentroCostoAsync(idCentroCosto);
                    
                    var activosFiltrados = activosCentro
                        .Where(a => a.ResponsableId != request.IdUsuario) // Excluir propios
                        .Where(a => a.Estatus == "Activo") // ? Filtrar solo activos
                        .OrderByDescending(a => a.FechaAdquisicion)
                        .Select(a => new ActivoResumenDto
                        {
                            IdActivo = a.IdActivo,
                            Nombre = a.Descripcion,
                            Categoria = a.CategoriaNavigation?.Nombre,
                            Responsable = $"{a.Responsable.Nombres} {a.Responsable.ApellidoPaterno}".Trim(),
                            CentroCosto = a.Responsable.CentroCosto != null 
                                ? $"{a.Responsable.CentroCosto.RazonSocial} - {a.Responsable.CentroCosto.Ubicacion}".Trim()
                                : null,
                            Estado = a.Estatus,
                            FechaAdquisicion = a.FechaAdquisicion,
                            ImagenUrl = a.ImagenUrl,
                            Marca = a.Marca,
                            Modelo = a.Modelo,
                            Etiqueta = a.Etiqueta
                        })
                        .ToList();

                    if (activosFiltrados.Any())
                    {
                        var nombreCentro = activosFiltrados.FirstOrDefault()?.CentroCosto ?? $"CentroCosto_{idCentroCosto}";
                        response.CentrosCosto[$"CentroCosto_{idCentroCosto}"] = activosFiltrados;
                    }
                }
            }

            return response;
        }
    }
}
