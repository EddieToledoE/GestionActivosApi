using GestionActivos.Application.SolicitudApplication.Commands;
using GestionActivos.Application.SolicitudApplication.DTOs;
using GestionActivos.Application.SolicitudApplication.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GestionActivos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SolicitudController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SolicitudController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSolicitudDto dto)
        {
            var id = await _mediator.Send(new CreateSolicitudCommand(dto));
            return Ok(new { message = "Solicitud creada correctamente", id });
        }

        [HttpGet("pendientes/emisor/{emisorId}")]
        public async Task<IActionResult> GetPendientesByEmisor(int emisorId)
        {
            var solicitudes = await _mediator.Send(new GetSolicitudesPendientesByEmisorQuery(emisorId));
            return Ok(solicitudes);
        }

        [HttpGet("pendientes/receptor/{receptorId}")]
        public async Task<IActionResult> GetPendientesByReceptor(int receptorId)
        {
            var solicitudes = await _mediator.Send(new GetSolicitudesPendientesByReceptorQuery(receptorId));
            return Ok(solicitudes);
        }

        [HttpPut("{id}/aceptar")]
        public async Task<IActionResult> Aceptar(int id, [FromBody] AceptarSolicitudRequest request)
        {
            await _mediator.Send(new AceptarSolicitudCommand(id, request.IdUsuarioAprobador));
            return Ok(new { message = "Solicitud aceptada correctamente" });
        }

        [HttpPut("{id}/rechazar")]
        public async Task<IActionResult> Rechazar(int id, [FromBody] RechazarSolicitudRequest request)
        {
            await _mediator.Send(new RechazarSolicitudCommand(id, request.IdUsuarioAprobador, request.MotivoRechazo));
            return Ok(new { message = "Solicitud rechazada correctamente" });
        }
    }

    /// <summary>
    /// Request DTO para aceptar una solicitud
    /// </summary>
    public class AceptarSolicitudRequest
    {
        public int IdUsuarioAprobador { get; set; }
    }

    /// <summary>
    /// Request DTO para rechazar una solicitud
    /// </summary>
    public class RechazarSolicitudRequest
    {
        public int IdUsuarioAprobador { get; set; }
        public string? MotivoRechazo { get; set; }
    }
}
