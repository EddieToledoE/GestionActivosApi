using GestionActivos.Application.SolicitudApplication.DTOs;
using GestionActivos.Application.SolicitudApplication.Queries;
using GestionActivos.Domain.Interfaces;
using MediatR;

namespace GestionActivos.Application.SolicitudApplication.Handlers
{
    /// <summary>
    /// Handler para obtener el conteo simple de solicitudes pendientes de un usuario.
    /// 
    /// Flujo:
    /// 1. Obtiene solicitudes entrantes (usuario como receptor) pendientes
    /// 2. Obtiene solicitudes salientes (usuario como emisor) pendientes
    /// 3. Cuenta cada grupo
    /// 4. Calcula el total
    /// </summary>
    public class GetConteoSolicitudesByUsuarioHandler : IRequestHandler<GetConteoSolicitudesByUsuarioQuery, ConteoSolicitudesDto>
    {
        private readonly ISolicitudRepository _solicitudRepository;

        public GetConteoSolicitudesByUsuarioHandler(ISolicitudRepository solicitudRepository)
        {
            _solicitudRepository = solicitudRepository;
        }

        public async Task<ConteoSolicitudesDto> Handle(
            GetConteoSolicitudesByUsuarioQuery request,
            CancellationToken cancellationToken)
        {
            // 1. Obtener solicitudes entrantes (usuario como receptor) y contar pendientes
            var solicitudesEntrantes = await _solicitudRepository.GetByReceptorIdAsync(request.IdUsuario);
            var conteoEntrantes = solicitudesEntrantes
                .Count(s => s.Estado.Equals("Pendiente", StringComparison.OrdinalIgnoreCase));

            // 2. Obtener solicitudes salientes (usuario como emisor) y contar pendientes
            var solicitudesSalientes = await _solicitudRepository.GetByEmisorIdAsync(request.IdUsuario);
            var conteoSalientes = solicitudesSalientes
                .Count(s => s.Estado.Equals("Pendiente", StringComparison.OrdinalIgnoreCase));

            // 3. Construir respuesta simple
            var response = new ConteoSolicitudesDto
            {
                Total = conteoEntrantes + conteoSalientes,
                Individual = new ConteoSolicitudesIndividualDto
                {
                    Entrantes = conteoEntrantes,
                    Salientes = conteoSalientes
                }
            };

            return response;
        }
    }
}
