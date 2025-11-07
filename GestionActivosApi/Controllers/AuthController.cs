using GestionActivos.Application.AuthApplication.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GestionActivos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var usuario = await _mediator.Send(command);

            if (usuario is null)
                return Unauthorized(new { message = "Credenciales incorrectas" });

            return Ok(
                new
                {
                    message = "Inicio de sesión exitoso",
                    usuario = new
                    {
                        usuario.IdUsuario,
                        usuario.Nombres,
                        usuario.ApellidoPaterno,
                        usuario.Correo,
                    },
                }
            );
        }
    }
}
