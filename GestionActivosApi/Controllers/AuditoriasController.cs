using GestionActivos.Application.AuditoriaApplication.Commands;
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
    /// También permite registrar y consultar auditorías completadas.
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

        /// <summary>
        /// Registra una nueva auditoría completa con sus detalles.
        /// </summary>
        /// <param name="dto">Datos de la auditoría a registrar</param>
        /// <returns>ID de la auditoría creada</returns>
        /// <response code="200">Auditoría registrada exitosamente</response>
        /// <response code="400">Datos inválidos</response>
        /// <response code="404">Usuario, activo o centro de costo no encontrado</response>
        [HttpPost("registrar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RegistrarAuditoria([FromBody] CrearAuditoriaDto dto)
        {
            var command = new CrearAuditoriaCommand(dto);
            var idAuditoria = await _mediator.Send(command);
            
            return Ok(new 
            { 
                idAuditoria = idAuditoria,
                mensaje = "Auditoría registrada correctamente." 
            });
        }

        /// <summary>
        /// Obtiene todas las auditorías de un centro de costo específico.
        /// </summary>
        /// <param name="idCentroCosto">ID del centro de costo</param>
        /// <returns>Lista de auditorías con sus detalles</returns>
        /// <response code="200">Retorna la lista de auditorías del centro</response>
        [HttpGet("centro/{idCentroCosto:int}")]
        [ProducesResponseType(typeof(IEnumerable<AuditoriaDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAuditoriasPorCentroCosto(int idCentroCosto)
        {
            var query = new GetAuditoriasPorCentroCostoQuery(idCentroCosto);
            var resultado = await _mediator.Send(query);
            return Ok(resultado);
        }
    }
}
