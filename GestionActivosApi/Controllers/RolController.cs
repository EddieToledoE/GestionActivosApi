using GestionActivos.Application.RolApplication.Commands;
using GestionActivos.Application.RolApplication.DTOs;
using GestionActivos.Application.RolApplication.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GestionActivos.API.Controllers
{
    /// <summary>
    /// Controlador para gestionar roles del sistema.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class RolController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RolController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Obtiene todos los roles con sus permisos.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RolDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _mediator.Send(new GetRolesQuery());
            return Ok(roles);
        }

        /// <summary>
        /// Crea un nuevo rol.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CrearRol([FromBody] CrearRolDto dto)
        {
            var idRol = await _mediator.Send(new CrearRolCommand(dto));
            return Ok(new 
            { 
                message = "Rol creado correctamente.",
                idRol = idRol
            });
        }

        /// <summary>
        /// Asigna permisos a un rol (reemplaza los permisos existentes).
        /// </summary>
        [HttpPost("asignar-permisos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AsignarPermisos([FromBody] AsignarPermisosDto dto)
        {
            await _mediator.Send(new AsignarPermisosCommand(dto));
            return Ok(new { message = "Permisos asignados correctamente al rol." });
        }
    }
}
