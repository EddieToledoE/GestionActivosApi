using GestionActivos.Application.AuditoriaApplication.DTOs;
using GestionActivos.Application.AuditoriaApplication.Queries;
using GestionActivos.Application.AuditoriaApplication.Services;
using GestionActivos.Domain.Exceptions;
using GestionActivos.Domain.Interfaces;
using MediatR;

namespace GestionActivos.Application.AuditoriaApplication.Handlers
{
    /// <summary>
    /// Handler que procesa la consulta del estado de auditoría de un usuario.
    /// 
    /// Flujo:
    /// 1. Obtiene el usuario por ID
    /// 2. Verifica que tenga centro de costo asignado
    /// 3. Obtiene la configuración activa de auditoría del centro
    /// 4. Obtiene la última auditoría realizada del usuario
    /// 5. Calcula si tiene auditoría pendiente usando AuditoriaService
    /// 6. Retorna el DTO con toda la información
    /// </summary>
    public class GetEstadoAuditoriaHandler : IRequestHandler<GetEstadoAuditoriaQuery, EstadoAuditoriaDto>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IConfigAuditoriaRepository _configAuditoriaRepository;
        private readonly IAuditoriaRepository _auditoriaRepository;
        private readonly AuditoriaService _auditoriaService;

        public GetEstadoAuditoriaHandler(
            IUsuarioRepository usuarioRepository,
            IConfigAuditoriaRepository configAuditoriaRepository,
            IAuditoriaRepository auditoriaRepository,
            AuditoriaService auditoriaService)
        {
            _usuarioRepository = usuarioRepository;
            _configAuditoriaRepository = configAuditoriaRepository;
            _auditoriaRepository = auditoriaRepository;
            _auditoriaService = auditoriaService;
        }

        public async Task<EstadoAuditoriaDto> Handle(
            GetEstadoAuditoriaQuery request,
            CancellationToken cancellationToken)
        {
            // 1. Obtener el usuario
            var usuario = await _usuarioRepository.GetByIdAsync(request.IdUsuario);
            if (usuario == null)
            {
                throw new NotFoundException($"No se encontró el usuario con ID {request.IdUsuario}.");
            }

            // 2. Verificar que tenga centro de costo
            if (usuario.IdCentroCosto == null)
            {
                return new EstadoAuditoriaDto
                {
                    Periodo = "N/A",
                    Pendiente = false,
                    Mensaje = "El usuario no tiene un centro de costo asignado.",
                    FechaUltimaAuditoria = null,
                    CentroCosto = null
                };
            }

            // 3. Obtener configuración activa del centro de costo
            var config = await _configAuditoriaRepository
                .GetConfiguracionActivaPorCentroCostoAsync(usuario.IdCentroCosto.Value);

            // 4. Obtener última auditoría del usuario
            var ultimaAuditoria = await _auditoriaRepository
                .GetUltimaAuditoriaPorUsuarioAsync(usuario.IdUsuario);

            // 5. Calcular si está pendiente usando el servicio de lógica de negocio
            bool pendiente = _auditoriaService.EsAuditoriaPendiente(config, ultimaAuditoria);
            string mensaje = _auditoriaService.ObtenerMensajeEstado(config, pendiente, ultimaAuditoria);

            // 6. Construir y retornar el DTO
            return new EstadoAuditoriaDto
            {
                Periodo = config?.Periodo ?? "No configurado",
                Pendiente = pendiente,
                Mensaje = mensaje,
                FechaUltimaAuditoria = ultimaAuditoria?.Fecha,
                CentroCosto = usuario.CentroCosto?.RazonSocial ?? usuario.CentroCosto?.Ubicacion
            };
        }
    }
}
