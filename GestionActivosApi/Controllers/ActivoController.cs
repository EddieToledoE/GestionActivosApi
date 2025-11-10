using GestionActivos.Application.ActivoApplication.Commands;
using GestionActivos.Application.ActivoApplication.DTOs;
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

      [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateActivoDto dto)
    {
        var id = await _mediator.Send(new CreateActivoCommand(dto));
      return Ok(new { message = "Activo creado correctamente", id });
 }
    }
}
