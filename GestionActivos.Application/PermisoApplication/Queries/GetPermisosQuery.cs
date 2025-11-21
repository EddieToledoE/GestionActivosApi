using GestionActivos.Application.RolApplication.DTOs;
using GestionActivos.Domain.Interfaces;
using MediatR;

namespace GestionActivos.Application.PermisoApplication.Queries
{
    public record GetPermisosQuery : IRequest<IEnumerable<PermisoDto>>;

    public class GetPermisosHandler : IRequestHandler<GetPermisosQuery, IEnumerable<PermisoDto>>
    {
        private readonly IPermisoRepository _permisoRepository;

        public GetPermisosHandler(IPermisoRepository permisoRepository)
        {
            _permisoRepository = permisoRepository;
        }

        public async Task<IEnumerable<PermisoDto>> Handle(GetPermisosQuery request, CancellationToken cancellationToken)
        {
            var permisos = await _permisoRepository.GetAllAsync();

            return permisos.Select(p => new PermisoDto
            {
                IdPermiso = p.IdPermiso,
                Nombre = p.Nombre
            }).ToList();
        }
    }
}
