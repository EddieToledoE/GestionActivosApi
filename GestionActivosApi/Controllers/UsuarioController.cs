using GestionActivos.Application.UsuarioApplication.Commands;
using GestionActivos.Application.UsuarioApplication.DTOs;
using GestionActivos.Application.UsuarioApplication.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GestionActivos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsuarioController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var usuarios = await _mediator.Send(new GetUsuariosQuery());
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var usuario = await _mediator.Send(new GetUsuarioByIdQuery(id));
            if (usuario is null)
                return NotFound();

            return Ok(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUsuarioDto dto)
        {
            var id = await _mediator.Send(new CreateUsuarioCommand(dto));
            return Ok(new { message = "Usuario creado correctamente", id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUsuarioDto dto)
        {
            if (id != dto.Id)
                return BadRequest();

            var success = await _mediator.Send(new UpdateUsuarioCommand(dto));
            return success ? Ok(new { message = "Usuario actualizado correctamente" }) : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _mediator.Send(new DeleteUsuarioCommand(id));
            return success ? Ok(new { message = "Usuario desactivado correctamente" }) : NotFound();
        }
    }
}
