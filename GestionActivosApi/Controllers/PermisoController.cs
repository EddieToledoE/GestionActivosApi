using GestionActivos.Application.PermisoApplication.Commands;
using GestionActivos.Application.PermisoApplication.DTOs;
using GestionActivos.Application.PermisoApplication.Queries;
using GestionActivos.Application.RolApplication.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GestionActivos.API.Controllers
{
    /// <summary>
    /// Controlador para gestionar permisos del sistema.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PermisoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PermisoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Obtiene todos los permisos disponibles.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PermisoDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPermisos()
        {
            var permisos = await _mediator.Send(new GetPermisosQuery());
            return Ok(permisos);
        }

        /// <summary>
        /// Crea un nuevo permiso.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CrearPermiso([FromBody] CrearPermisoDto dto)
        {
            var idPermiso = await _mediator.Send(new CrearPermisoCommand(dto));
            return Ok(new 
            { 
                message = "Permiso creado correctamente.",
                idPermiso = idPermiso
            });
        }
    }
}
