using GestionActivos.Application.ActivoApplication.Commands;
using GestionActivos.Application.ActivoApplication.DTOs;
using GestionActivos.Application.ActivoApplication.Queries;
using GestionActivos.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GestionActivos.API.Controllers
{
    /// <summary>
    /// Controlador para gestionar activos del sistema.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ActivoController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUsuarioRolRepository _usuarioRolRepository;
        private readonly IUsuarioCentroCostoRepository _usuarioCentroCostoRepository;

        public ActivoController(
            IMediator mediator,
            IUsuarioRolRepository usuarioRolRepository,
            IUsuarioCentroCostoRepository usuarioCentroCostoRepository)
        {
            _mediator = mediator;
            _usuarioRolRepository = usuarioRolRepository;
            _usuarioCentroCostoRepository = usuarioCentroCostoRepository;
        }

        /// <summary>
        /// Obtiene activos agrupados según el contexto del usuario autenticado.
        /// Requiere headers: X-User-Id (obligatorio)
        ///
        /// Respuesta:
        /// - Activos_Propios: Activos asignados al usuario
        /// - CentroCosto_X: Activos por centro de costo (solo si tiene permiso "Activos_Ver_Externos")
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ActivosAgrupadosResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetActivosAgrupados()
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

            // 2. Verificar si el usuario tiene el permiso "Activos_Ver_Externos"
            var rolesUsuario = await _usuarioRolRepository.GetRolesByUsuarioIdAsync(idUsuario);
            var tienePermisoExterno = rolesUsuario
                .SelectMany(ur => ur.Rol.Permisos)
                .Any(rp =>
                    rp.Permiso.Nombre.Equals(
                        "Activo_Ver_Externos",
                        StringComparison.OrdinalIgnoreCase
                    )
                );

            // 3. Obtener centros de costo si tiene permiso
            List<int> idsCentrosCosto = new();
            if (tienePermisoExterno)
            {
                var centrosCosto = await _usuarioCentroCostoRepository.GetByUsuarioIdAsync(
                    idUsuario
                );
                idsCentrosCosto = centrosCosto
                    .Where(ucc => ucc.Activo)
                    .Select(ucc => ucc.IdCentroCosto)
                    .ToList();
            }

            // 4. Ejecutar query
            var query = new GetActivosAgrupadosQuery(idUsuario, idsCentrosCosto);
            var resultado = await _mediator.Send(query);

            // 5. Validar si hay activos
            if (!resultado.Activos_Propios.Any() && !resultado.CentrosCosto.Any())
            {
                return NotFound(
                    new { message = "No se encontraron activos disponibles para este usuario." }
                );
            }

            return Ok(resultado);
        }

        /// <summary>
        /// Obtiene un activo por su ID.
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var activo = await _mediator.Send(new GetActivoByIdQuery(id));
            return Ok(activo);
        }

        /// <summary>
        /// Obtiene un activo con su historial completo de reubicaciones.
        /// Útil para trazabilidad y auditoría de movimientos del activo.
        /// </summary>
        /// <param name="id">ID del activo</param>
        /// <returns>Activo con historial de reubicaciones ordenado por fecha descendente</returns>
        [HttpGet("{id:guid}/historial")]
        [ProducesResponseType(typeof(ActivoConHistorialDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetActivoConHistorial(Guid id)
        {
            var resultado = await _mediator.Send(new GetActivoConHistorialQuery(id));
            return Ok(resultado);
        }

        /// <summary>
        /// Obtiene activos por responsable.
        /// </summary>
        [HttpGet("responsable/{responsableId:guid}")]
        public async Task<IActionResult> GetByResponsableId(Guid responsableId)
        {
            var activos = await _mediator.Send(new GetActivosByResponsableIdQuery(responsableId));
            return Ok(activos);
        }

        /// <summary>
        /// Crea un nuevo activo.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateActivoDto dto)
        {
            var id = await _mediator.Send(new CreateActivoCommand(dto));
            return Ok(new { message = "Activo creado correctamente", id });
        }

        /// <summary>
        /// Desactiva un activo (soft delete).
        /// </summary>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteActivoCommand(id));
            return Ok(new { message = "Activo desactivado correctamente" });
        }
    }
}
