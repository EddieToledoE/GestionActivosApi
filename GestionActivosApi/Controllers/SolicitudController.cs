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

        [HttpGet("pendientes/emisor/{emisorId:guid}")]
        public async Task<IActionResult> GetPendientesByEmisor(Guid emisorId)
        {
            var solicitudes = await _mediator.Send(new GetSolicitudesPendientesByEmisorQuery(emisorId));
            return Ok(solicitudes);
        }

        [HttpGet("pendientes/receptor/{receptorId:guid}")]
        public async Task<IActionResult> GetPendientesByReceptor(Guid receptorId)
        {
            var solicitudes = await _mediator.Send(new GetSolicitudesPendientesByReceptorQuery(receptorId));
            return Ok(solicitudes);
        }

        [HttpPut("{id:guid}/aceptar")]
        public async Task<IActionResult> Aceptar(Guid id, [FromBody] AceptarSolicitudRequest request)
        {
            await _mediator.Send(new AceptarSolicitudCommand(id, request.IdUsuarioAprobador));
            return Ok(new { message = "Solicitud aceptada correctamente" });
        }

        [HttpPut("{id:guid}/rechazar")]
        public async Task<IActionResult> Rechazar(Guid id, [FromBody] RechazarSolicitudRequest request)
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
        public Guid IdUsuarioAprobador { get; set; }
    }

    /// <summary>
    /// Request DTO para rechazar una solicitud
    /// </summary>
    public class RechazarSolicitudRequest
    {
        public Guid IdUsuarioAprobador { get; set; }
        public string? MotivoRechazo { get; set; }
    }
}
