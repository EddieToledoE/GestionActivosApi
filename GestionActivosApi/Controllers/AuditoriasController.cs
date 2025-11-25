using GestionActivos.Application.AuditoriaApplication.Commands;
using GestionActivos.Application.AuditoriaApplication.DTOs;
using GestionActivos.Application.AuditoriaApplication.Queries;
using GestionActivos.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GestionActivos.API.Controllers
{
    /// <summary>
    /// Controlador para gestionar las auditorías de usuarios.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuditoriasController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUsuarioRolRepository _usuarioRolRepository;
        private readonly IUsuarioCentroCostoRepository _usuarioCentroCostoRepository;

        public AuditoriasController(
            IMediator mediator,
            IUsuarioRolRepository usuarioRolRepository,
            IUsuarioCentroCostoRepository usuarioCentroCostoRepository)
        {
            _mediator = mediator;
            _usuarioRolRepository = usuarioRolRepository;
            _usuarioCentroCostoRepository = usuarioCentroCostoRepository;
        }

        /// <summary>
        /// Obtiene auditorías agrupadas por centro de costo y tipo.
        /// Requiere header X-User-Id y permiso "Auditoria_Ver_Externos".
        /// 
        /// Respuesta agrupada:
        /// - Por centro de costo
        /// - Por tipo (Auto/Externa)
        /// - Ordenadas por fecha descendente
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(AuditoriasAgrupadasResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAuditoriasAgrupadas()
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

            // 2. Verificar si el usuario tiene el permiso "Auditoria_Ver_Externos"
            var rolesUsuario = await _usuarioRolRepository.GetRolesByUsuarioIdAsync(idUsuario);
            var tienePermisoExterno = rolesUsuario
                .SelectMany(ur => ur.Rol.Permisos)
                .Any(rp => rp.Permiso.Nombre.Equals("Auditoria_Ver_Externos", StringComparison.OrdinalIgnoreCase));

            if (!tienePermisoExterno)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new 
                { 
                    error = "No tienes permiso para ver auditorías externas.",
                    permisoRequerido = "Auditoria_Ver_Externos"
                });
            }

            // 3. Obtener centros de costo accesibles
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

            // 4. Ejecutar query
            var query = new GetAuditoriasAgrupadasQuery(idUsuario, idsCentrosCosto);
            var resultado = await _mediator.Send(query);

            // 5. Validar si hay auditorías
            if (!resultado.CentrosCosto.Any())
            {
                return NotFound(new 
                { 
                    message = "No se encontraron auditorías en los centros de costo accesibles." 
                });
            }

            return Ok(resultado);
        }

        /// <summary>
        /// Obtiene una auditoría completa con sus detalles.
        /// Incluye información del auditor, usuario auditado, centro de costo
        /// y todos los activos auditados con su estado.
        /// </summary>
        /// <param name="id">ID de la auditoría</param>
        /// <returns>Auditoría con detalles completos</returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(AuditoriaConDetallesDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAuditoriaConDetalles(Guid id)
        {
            var resultado = await _mediator.Send(new GetAuditoriaConDetallesQuery(id));
            return Ok(resultado);
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
