using GestionActivos.Application.AuthApplication.Commands;
using GestionActivos.Application.AuthApplication.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GestionActivos.API.Controllers
{
    /// <summary>
    /// Controlador de autenticación.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Endpoint de login que retorna información completa del usuario:
        /// - Datos personales
        /// - Roles y permisos (para control de UI)
        /// - Centros de costo con acceso (para filtrado de datos en backend)
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var loginResponse = await _mediator.Send(command);
            return Ok(loginResponse);
        }
    }
}
