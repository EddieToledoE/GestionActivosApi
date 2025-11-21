using GestionActivos.Application.UsuarioApplication.DTOs;
using GestionActivos.Domain.Entities;
using GestionActivos.Domain.Exceptions;
using GestionActivos.Domain.Interfaces;
using MediatR;

namespace GestionActivos.Application.UsuarioApplication.Commands
{
    public record AsignarRolCommand(AsignarRolDto Dto) : IRequest<bool>;

    public class AsignarRolHandler : IRequestHandler<AsignarRolCommand, bool>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IRolRepository _rolRepository;
        private readonly IUsuarioRolRepository _usuarioRolRepository;

        public AsignarRolHandler(
            IUsuarioRepository usuarioRepository,
            IRolRepository rolRepository,
            IUsuarioRolRepository usuarioRolRepository)
        {
            _usuarioRepository = usuarioRepository;
            _rolRepository = rolRepository;
            _usuarioRolRepository = usuarioRolRepository;
        }

        public async Task<bool> Handle(AsignarRolCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            // Validar que el usuario existe
            var usuario = await _usuarioRepository.GetByIdAsync(dto.IdUsuario);
            if (usuario == null)
            {
                throw new NotFoundException($"No se encontró el usuario con ID {dto.IdUsuario}.");
            }

            if (!usuario.Activo)
            {
                throw new BusinessException("No se pueden asignar roles a usuarios inactivos.");
            }

            // Validar que el rol existe
            var rol = await _rolRepository.GetByIdAsync(dto.IdRol);
            if (rol == null)
            {
                throw new NotFoundException($"No se encontró el rol con ID {dto.IdRol}.");
            }

            // Validar que no exista ya la asignación
            var existeAsignacion = await _usuarioRolRepository.ExistsAsync(dto.IdUsuario, dto.IdRol);
            if (existeAsignacion)
            {
                throw new BusinessException($"El usuario ya tiene asignado el rol '{rol.Nombre}'.");
            }

            // Crear asignación
            var usuarioRol = new UsuarioRol
            {
                IdUsuario = dto.IdUsuario,
                IdRol = dto.IdRol
            };

            await _usuarioRolRepository.AddAsync(usuarioRol);
            return true;
        }
    }
}
