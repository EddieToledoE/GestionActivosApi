using GestionActivos.Application.ReubicacionApplication.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GestionActivos.API.Controllers
{
    /// <summary>
    /// Controlador para gestionar las reubicaciones de activos.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ReubicacionesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReubicacionesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Permite a un auditor transferir directamente un activo de un usuario a otro.
        /// Esta operación no requiere aprobación y genera notificaciones automáticas.
        /// </summary>
        /// <param name="command">Datos de la transferencia</param>
        /// <returns>Resultado de la operación</returns>
        /// <response code="200">Transferencia realizada correctamente</response>
        /// <response code="400">Error de validación o reglas de negocio</response>
        /// <response code="404">Entidad no encontrada</response>
        [HttpPost("auditor-transfer")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AuditorTransfer([FromBody] AuditorTransferCommand command)
        {
            var result = await _mediator.Send(command);
            
            return Ok(new 
            { 
                success = result, 
                message = "Transferencia realizada correctamente." 
            });
        }
    }
}
