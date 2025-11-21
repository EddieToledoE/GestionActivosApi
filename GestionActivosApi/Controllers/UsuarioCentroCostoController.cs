using GestionActivos.Application.UsuarioCentroCostoApplication.DTOs;
using GestionActivos.Domain.Entities;
using GestionActivos.Domain.Exceptions;
using GestionActivos.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GestionActivos.API.Controllers
{
    /// <summary>
    /// Controlador para gestionar la relación muchos-a-muchos entre Usuario y CentroCosto.
    /// Permite asignar usuarios a múltiples centros de costo (empresas/ubicaciones).
    /// </summary>
    [ApiController]
    [Route("api/usuario-centrocosto")]
    public class UsuarioCentroCostoController : ControllerBase
    {
        private readonly IUsuarioCentroCostoRepository _repository;
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioCentroCostoController(
            IUsuarioCentroCostoRepository repository,
            IUsuarioRepository usuarioRepository)
        {
            _repository = repository;
            _usuarioRepository = usuarioRepository;
        }

        /// <summary>
        /// Obtiene todos los centros de costo asignados a un usuario.
        /// </summary>
        /// <param name="idUsuario">GUID del usuario</param>
        /// <returns>Lista de centros de costo del usuario</returns>
        [HttpGet("{idUsuario:guid}")]
        [ProducesResponseType(typeof(IEnumerable<UsuarioCentroCostoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCentrosCostoPorUsuario(Guid idUsuario)
        {
            // Validar que el usuario existe
            var usuario = await _usuarioRepository.GetByIdAsync(idUsuario);
            if (usuario == null)
            {
                return NotFound(new { message = $"No se encontró el usuario con ID {idUsuario}." });
            }

            var asignaciones = await _repository.GetByUsuarioIdAsync(idUsuario);

            var result = asignaciones.Select(uc => new UsuarioCentroCostoDto
            {
                IdUsuario = uc.IdUsuario,
                NombreUsuario = $"{uc.Usuario.Nombres} {uc.Usuario.ApellidoPaterno}".Trim(),
                IdCentroCosto = uc.IdCentroCosto,
                RazonSocial = uc.CentroCosto.RazonSocial,
                Ubicacion = uc.CentroCosto.Ubicacion,
                Area = uc.CentroCosto.Area,
                FechaInicio = uc.FechaInicio,
                FechaFin = uc.FechaFin,
                Activo = uc.Activo
            }).ToList();

            return Ok(result);
        }

        /// <summary>
        /// Asigna un centro de costo a un usuario.
        /// </summary>
        /// <param name="dto">Datos de la asignación</param>
        /// <returns>Confirmación de la asignación</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> AsignarCentroCosto([FromBody] AsignarCentroCostoDto dto)
        {
            // Validar que el usuario existe
            var usuario = await _usuarioRepository.GetByIdAsync(dto.IdUsuario);
            if (usuario == null)
            {
                return NotFound(new { message = $"No se encontró el usuario con ID {dto.IdUsuario}." });
            }

            if (!usuario.Activo)
            {
                return BadRequest(new { message = "No se pueden asignar centros de costo a usuarios inactivos." });
            }

            // Validar que no exista ya una asignación activa
            var existeAsignacion = await _repository.ExistsAsync(dto.IdUsuario, dto.IdCentroCosto);
            if (existeAsignacion)
            {
                return Conflict(new 
                { 
                    message = $"Ya existe una asignación activa del usuario al centro de costo con ID {dto.IdCentroCosto}." 
                });
            }

            // Crear nueva asignación
            var asignacion = new UsuarioCentroCosto
            {
                IdUsuario = dto.IdUsuario,
                IdCentroCosto = dto.IdCentroCosto,
                FechaInicio = dto.FechaInicio ?? DateTime.Now,
                Activo = true
            };

            await _repository.AddAsync(asignacion);

            return Ok(new 
            { 
                message = "Centro de costo asignado correctamente.",
                data = new
                {
                    idUsuario = asignacion.IdUsuario,
                    idCentroCosto = asignacion.IdCentroCosto,
                    fechaInicio = asignacion.FechaInicio
                }
            });
        }

        /// <summary>
        /// Elimina la asignación de un centro de costo a un usuario (soft delete).
        /// </summary>
        /// <param name="idUsuario">GUID del usuario</param>
        /// <param name="idCentroCosto">ID del centro de costo</param>
        /// <returns>Confirmación de la eliminación</returns>
        [HttpDelete("{idUsuario:guid}/{idCentroCosto:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoverAsignacion(Guid idUsuario, int idCentroCosto)
        {
            var asignacion = await _repository.GetByIdAsync(idUsuario, idCentroCosto);
            if (asignacion == null)
            {
                return NotFound(new 
                { 
                    message = $"No se encontró la asignación entre el usuario {idUsuario} y el centro de costo {idCentroCosto}." 
                });
            }

            if (!asignacion.Activo)
            {
                return BadRequest(new { message = "La asignación ya está inactiva." });
            }

            await _repository.RemoveAsync(idUsuario, idCentroCosto);

            return Ok(new 
            { 
                message = "Asignación de centro de costo eliminada correctamente." 
            });
        }
    }
}
