using GestionActivos.Application.ActivoApplication.Commands;
using GestionActivos.Application.ActivoApplication.DTOs;
using GestionActivos.Application.ActivoApplication.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GestionActivos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActivoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ActivoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var activos = await _mediator.Send(new GetActivosQuery());
            return Ok(activos);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var activo = await _mediator.Send(new GetActivoByIdQuery(id));
            return Ok(activo);
        }

        [HttpGet("responsable/{responsableId:guid}")]
        public async Task<IActionResult> GetByResponsableId(Guid responsableId)
        {
            var activos = await _mediator.Send(new GetActivosByResponsableIdQuery(responsableId));
            return Ok(activos);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateActivoDto dto)
        {
            var id = await _mediator.Send(new CreateActivoCommand(dto));
            return Ok(new { message = "Activo creado correctamente", id });
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteActivoCommand(id));
            return Ok(new { message = "Activo desactivado correctamente" });
        }
    }
}
