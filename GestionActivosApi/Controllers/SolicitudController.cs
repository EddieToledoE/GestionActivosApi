using GestionActivos.Application.SolicitudApplication.Commands;
using GestionActivos.Application.SolicitudApplication.DTOs;
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
    }
}
