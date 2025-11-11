using AutoMapper;
using GestionActivos.Application.ActivoApplication.DTOs;
using GestionActivos.Domain.Interfaces;
using MediatR;

namespace GestionActivos.Application.ActivoApplication.Queries
{
    public record GetActivosByResponsableIdQuery(int ResponsableId) : IRequest<IEnumerable<ActivoDto>>;

    public class GetActivosByResponsableIdHandler : IRequestHandler<GetActivosByResponsableIdQuery, IEnumerable<ActivoDto>>
    {
        private readonly IActivoRepository _activoRepository;
  private readonly IMapper _mapper;

        public GetActivosByResponsableIdHandler(IActivoRepository activoRepository, IMapper mapper)
        {
            _activoRepository = activoRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ActivoDto>> Handle(
       GetActivosByResponsableIdQuery request,
          CancellationToken cancellationToken
        )
  {
     var activos = await _activoRepository.GetByResponsableIdAsync(request.ResponsableId);
            return _mapper.Map<IEnumerable<ActivoDto>>(activos);
        }
    }
}
