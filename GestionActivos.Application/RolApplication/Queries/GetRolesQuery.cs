using GestionActivos.Application.RolApplication.DTOs;
using GestionActivos.Domain.Interfaces;
using MediatR;

namespace GestionActivos.Application.RolApplication.Queries
{
    public record GetRolesQuery : IRequest<IEnumerable<RolDto>>;

    public class GetRolesHandler : IRequestHandler<GetRolesQuery, IEnumerable<RolDto>>
    {
        private readonly IRolRepository _rolRepository;

        public GetRolesHandler(IRolRepository rolRepository)
        {
            _rolRepository = rolRepository;
        }

        public async Task<IEnumerable<RolDto>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = await _rolRepository.GetAllAsync();

            return roles.Select(r => new RolDto
            {
                IdRol = r.IdRol,
                Nombre = r.Nombre,
                Permisos = r.Permisos.Select(rp => new PermisoDto
                {
                    IdPermiso = rp.Permiso.IdPermiso,
                    Nombre = rp.Permiso.Nombre
                }).ToList()
            }).ToList();
        }
    }
}
