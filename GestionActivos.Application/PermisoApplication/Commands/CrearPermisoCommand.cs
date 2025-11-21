using GestionActivos.Application.PermisoApplication.DTOs;
using GestionActivos.Domain.Entities;
using GestionActivos.Domain.Exceptions;
using GestionActivos.Domain.Interfaces;
using MediatR;

namespace GestionActivos.Application.PermisoApplication.Commands
{
    public record CrearPermisoCommand(CrearPermisoDto Permiso) : IRequest<int>;

    public class CrearPermisoHandler : IRequestHandler<CrearPermisoCommand, int>
    {
        private readonly IPermisoRepository _permisoRepository;

        public CrearPermisoHandler(IPermisoRepository permisoRepository)
        {
            _permisoRepository = permisoRepository;
        }

        public async Task<int> Handle(CrearPermisoCommand request, CancellationToken cancellationToken)
        {
            // Validar que no exista un permiso con el mismo nombre
            var existePermiso = await _permisoRepository.ExistsByNombreAsync(request.Permiso.Nombre);
            if (existePermiso)
            {
                throw new BusinessException($"Ya existe un permiso con el nombre '{request.Permiso.Nombre}'.");
            }

            var permiso = new Permiso
            {
                Nombre = request.Permiso.Nombre
            };

            await _permisoRepository.AddAsync(permiso);
            return permiso.IdPermiso;
        }
    }
}
