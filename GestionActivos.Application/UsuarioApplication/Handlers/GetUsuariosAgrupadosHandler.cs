using GestionActivos.Application.UsuarioApplication.DTOs;
using GestionActivos.Application.UsuarioApplication.Queries;
using GestionActivos.Domain.Interfaces;
using MediatR;

namespace GestionActivos.Application.UsuarioApplication.Handlers
{
    /// <summary>
    /// Handler para obtener usuarios agrupados por centro de costo.
    /// 
    /// Flujo:
    /// 1. Filtra usuarios por centros de costo accesibles
    /// 2. Aplica búsqueda opcional (nombre, apellidos, claveFortia)
    /// 3. Agrupa por centro de costo
    /// 4. Ordena alfabéticamente por nombre completo
    /// 5. Proyecta a DTOs sin información sensible
    /// </summary>
    public class GetUsuariosAgrupadosHandler : IRequestHandler<GetUsuariosAgrupadosQuery, UsuariosAgrupadosResponseDto>
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public GetUsuariosAgrupadosHandler(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<UsuariosAgrupadosResponseDto> Handle(
            GetUsuariosAgrupadosQuery request,
            CancellationToken cancellationToken)
        {
            var response = new UsuariosAgrupadosResponseDto();

            // Si no tiene acceso a ningún centro, retornar vacío
            if (request.IdsCentrosCostoAcceso == null || !request.IdsCentrosCostoAcceso.Any())
            {
                return response;
            }

            // Obtener usuarios por centros de costo accesibles
            var usuarios = await _usuarioRepository.GetUsuariosByCentrosCostoAsync(request.IdsCentrosCostoAcceso);

            // Aplicar búsqueda opcional
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var searchLower = request.SearchTerm.ToLower();
                usuarios = usuarios.Where(u =>
                    (u.Nombres?.ToLower().Contains(searchLower) ?? false) ||
                    (u.ApellidoPaterno?.ToLower().Contains(searchLower) ?? false) ||
                    (u.ApellidoMaterno?.ToLower().Contains(searchLower) ?? false) ||
                    (u.ClaveFortia?.ToLower().Contains(searchLower) ?? false)
                ).ToList();
            }

            // Agrupar por centro de costo
            var usuariosPorCentro = usuarios
                .Where(u => u.IdCentroCosto.HasValue) // Solo usuarios con centro asignado
                .GroupBy(u => u.IdCentroCosto!.Value);

            foreach (var grupoCentro in usuariosPorCentro)
            {
                var idCentroCosto = grupoCentro.Key;
                
                // Obtener información del centro de costo
                var primerUsuario = grupoCentro.FirstOrDefault();
                var centro = primerUsuario?.CentroCosto;

                // ? Construir clave con formato: RazonSocial_Ubicacion_Area
                string nombreCentro;
                if (centro != null)
                {
                    var razonSocial = !string.IsNullOrEmpty(centro.RazonSocial) ? centro.RazonSocial : "SinRazonSocial";
                    var ubicacion = !string.IsNullOrEmpty(centro.Ubicacion) ? centro.Ubicacion : "SinUbicacion";
                    var area = !string.IsNullOrEmpty(centro.Area) ? centro.Area : "SinArea";
                    
                    nombreCentro = $"{razonSocial}_{ubicacion}_{area}";
                }
                else
                {
                    nombreCentro = $"CentroCosto_{idCentroCosto}";
                }

                // Proyectar a DTOs y ordenar alfabéticamente
                var usuariosDto = grupoCentro
                    .OrderBy(u => u.Nombres)
                    .ThenBy(u => u.ApellidoPaterno)
                    .ThenBy(u => u.ApellidoMaterno)
                    .Select(u => new UsuarioResumenDto
                    {
                        IdUsuario = u.IdUsuario,
                        NombreCompleto = $"{u.Nombres} {u.ApellidoPaterno} {u.ApellidoMaterno}".Trim(),
                        ClaveFortia = u.ClaveFortia,
                        Correo = u.Correo
                    })
                    .ToList();

                response.CentrosCosto[nombreCentro] = usuariosDto;
            }

            return response;
        }
    }
}
