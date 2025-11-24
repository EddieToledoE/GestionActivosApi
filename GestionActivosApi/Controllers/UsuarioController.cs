using GestionActivos.Application.UsuarioApplication.Commands;
using GestionActivos.Application.UsuarioApplication.DTOs;
using GestionActivos.Application.UsuarioApplication.Queries;
using GestionActivos.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GestionActivos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUsuarioCentroCostoRepository _usuarioCentroCostoRepository;

        public UsuarioController(
            IMediator mediator,
            IUsuarioCentroCostoRepository usuarioCentroCostoRepository)
        {
            _mediator = mediator;
            _usuarioCentroCostoRepository = usuarioCentroCostoRepository;
        }

        /// <summary>
        /// Obtiene usuarios agrupados por centro de costo.
        /// Requiere header X-User-Id.
        /// Filtrado por centros de costo accesibles al usuario solicitante.
        /// Soporta búsqueda opcional mediante query param "search".
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(UsuariosAgrupadosResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUsuariosAgrupados([FromQuery] string? search = null)
        {
            // 1. Obtener X-User-Id del header
            if (!Request.Headers.TryGetValue("X-User-Id", out var userIdHeader))
            {
                return BadRequest(new { error = "Header 'X-User-Id' es requerido." });
            }

            if (!Guid.TryParse(userIdHeader.ToString(), out var idUsuario))
            {
                return BadRequest(new { error = "El valor de 'X-User-Id' no es un GUID válido." });
            }

            // 2. Obtener centros de costo accesibles
            var centrosCosto = await _usuarioCentroCostoRepository.GetByUsuarioIdAsync(idUsuario);
            var idsCentrosCosto = centrosCosto
                .Where(ucc => ucc.Activo)
                .Select(ucc => ucc.IdCentroCosto)
                .ToList();

            if (!idsCentrosCosto.Any())
            {
                return NotFound(new 
                { 
                    message = "No tienes acceso a ningún centro de costo." 
                });
            }

            // 3. Ejecutar query
            var query = new GetUsuariosAgrupadosQuery(idUsuario, idsCentrosCosto, search);
            var resultado = await _mediator.Send(query);

            // 4. Validar si hay usuarios
            if (!resultado.CentrosCosto.Any())
            {
                return Ok(new Dictionary<string, List<UsuarioResumenDto>>()); // Retornar objeto vacío
            }

            return Ok(resultado.CentrosCosto);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
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

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUsuarioDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest(new { error = "El ID de la URL no coincide con el ID del cuerpo de la petición." });
            }

            await _mediator.Send(new UpdateUsuarioCommand(dto));
            return Ok(new { message = "Usuario actualizado correctamente" });
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteUsuarioCommand(id));
            return Ok(new { message = "Usuario desactivado correctamente" });
        }

        /// <summary>
        /// Asigna un rol a un usuario.
        /// </summary>
        [HttpPost("asignar-rol")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AsignarRol([FromBody] AsignarRolDto dto)
        {
            await _mediator.Send(new AsignarRolCommand(dto));
            return Ok(new { message = "Rol asignado correctamente al usuario." });
        }
    }
}
