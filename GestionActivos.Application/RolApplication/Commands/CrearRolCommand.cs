using GestionActivos.Application.RolApplication.DTOs;
using GestionActivos.Domain.Entities;
using GestionActivos.Domain.Exceptions;
using GestionActivos.Domain.Interfaces;
using MediatR;

namespace GestionActivos.Application.RolApplication.Commands
{
    public record CrearRolCommand(CrearRolDto Rol) : IRequest<int>;

    public class CrearRolHandler : IRequestHandler<CrearRolCommand, int>
    {
        private readonly IRolRepository _rolRepository;

        public CrearRolHandler(IRolRepository rolRepository)
        {
            _rolRepository = rolRepository;
        }

        public async Task<int> Handle(CrearRolCommand request, CancellationToken cancellationToken)
        {
            // Validar que no exista un rol con el mismo nombre
            var existeRol = await _rolRepository.ExistsByNombreAsync(request.Rol.Nombre);
            if (existeRol)
            {
                throw new BusinessException($"Ya existe un rol con el nombre '{request.Rol.Nombre}'.");
            }

            var rol = new Rol
            {
                Nombre = request.Rol.Nombre
            };

            await _rolRepository.AddAsync(rol);
            return rol.IdRol;
        }
    }
}
