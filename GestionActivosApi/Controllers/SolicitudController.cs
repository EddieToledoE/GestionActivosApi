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
    }
}
