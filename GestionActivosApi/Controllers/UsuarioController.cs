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

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var usuario = await _mediator.Send(new GetUsuarioByIdQuery(id));
            return Ok(usuario);
        }

        [HttpGet("claveFortia/{claveFortia}")]
        public async Task<IActionResult> GetByClaveFortia(string claveFortia)
        {
            var usuario = await _mediator.Send(new GetUsuarioByClaveFortiaQuery(claveFortia));
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
            {
                return BadRequest(new { error = "El ID de la URL no coincide con el ID del cuerpo de la petición." });
            }

            await _mediator.Send(new UpdateUsuarioCommand(dto));
            return Ok(new { message = "Usuario actualizado correctamente" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteUsuarioCommand(id));
            return Ok(new { message = "Usuario desactivado correctamente" });
        }
    }
}
