using AutoMapper;
using GestionActivos.Application.ActivoApplication.DTOs;
using GestionActivos.Domain.Interfaces;
using MediatR;

namespace GestionActivos.Application.ActivoApplication.Queries
{
    public record GetActivosQuery : IRequest<IEnumerable<ActivoDto>>;

    public class GetActivosHandler : IRequestHandler<GetActivosQuery, IEnumerable<ActivoDto>>
    {
        private readonly IActivoRepository _activoRepository;
        private readonly IMapper _mapper;

        public GetActivosHandler(IActivoRepository activoRepository, IMapper mapper)
{
          _activoRepository = activoRepository;
      _mapper = mapper;
        }

 public async Task<IEnumerable<ActivoDto>> Handle(
   GetActivosQuery request,
     CancellationToken cancellationToken
   )
  {
      var activos = await _activoRepository.GetAllAsync();
     return _mapper.Map<IEnumerable<ActivoDto>>(activos);
        }
    }
}
