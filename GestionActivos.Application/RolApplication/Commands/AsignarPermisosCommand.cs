using GestionActivos.Application.RolApplication.DTOs;
using GestionActivos.Domain.Entities;
using GestionActivos.Domain.Exceptions;
using GestionActivos.Domain.Interfaces;
using MediatR;

namespace GestionActivos.Application.RolApplication.Commands
{
    public record AsignarPermisosCommand(AsignarPermisosDto Dto) : IRequest<bool>;

    public class AsignarPermisosHandler : IRequestHandler<AsignarPermisosCommand, bool>
    {
        private readonly IRolRepository _rolRepository;
        private readonly IPermisoRepository _permisoRepository;
        private readonly IRolPermisoRepository _rolPermisoRepository;

        public AsignarPermisosHandler(
            IRolRepository rolRepository,
            IPermisoRepository permisoRepository,
            IRolPermisoRepository rolPermisoRepository)
        {
            _rolRepository = rolRepository;
            _permisoRepository = permisoRepository;
            _rolPermisoRepository = rolPermisoRepository;
        }

        public async Task<bool> Handle(AsignarPermisosCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            // Validar que el rol existe
            var rol = await _rolRepository.GetByIdAsync(dto.IdRol);
            if (rol == null)
            {
                throw new NotFoundException($"No se encontró el rol con ID {dto.IdRol}.");
            }

            // Validar que todos los permisos existan
            var permisos = await _permisoRepository.GetByIdsAsync(dto.IdsPermisos);
            var permisosEncontrados = permisos.Count();
            if (permisosEncontrados != dto.IdsPermisos.Count)
            {
                throw new BusinessException("Uno o más permisos especificados no existen.");
            }

            // Eliminar permisos actuales del rol
            await _rolPermisoRepository.RemoveAllByRolIdAsync(dto.IdRol);

            // Asignar nuevos permisos
            foreach (var idPermiso in dto.IdsPermisos)
            {
                var rolPermiso = new RolPermiso
                {
                    IdRol = dto.IdRol,
                    IdPermiso = idPermiso
                };
                await _rolPermisoRepository.AddAsync(rolPermiso);
            }

            return true;
        }
    }
}
