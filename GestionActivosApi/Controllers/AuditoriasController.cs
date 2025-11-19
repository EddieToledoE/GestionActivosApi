using GestionActivos.Application.AuditoriaApplication.DTOs;
using GestionActivos.Application.AuditoriaApplication.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GestionActivos.API.Controllers
{
    /// <summary>
    /// Controlador para gestionar las auditorías de usuarios.
    /// Determina dinámicamente si un usuario tiene auditorías pendientes
    /// según la configuración de su centro de costo.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuditoriasController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuditoriasController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Obtiene el estado de auditoría de un usuario específico.
        /// </summary>
        /// <param name="idUsuario">GUID del usuario</param>
        /// <returns>DTO con el estado de auditoría (periodo, pendiente, mensaje)</returns>
        /// <response code="200">Retorna el estado de auditoría del usuario</response>
        /// <response code="404">Si el usuario no existe</response>
        [HttpGet("estado/{idUsuario:guid}")]
        [ProducesResponseType(typeof(EstadoAuditoriaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEstadoAuditoria(Guid idUsuario)
        {
            var query = new GetEstadoAuditoriaQuery(idUsuario);
            var resultado = await _mediator.Send(query);
            return Ok(resultado);
        }
    }
}
