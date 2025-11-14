using AutoMapper;
using GestionActivos.Application.ActivoApplication.DTOs;
using GestionActivos.Domain.Exceptions;
using GestionActivos.Domain.Interfaces;
using MediatR;

namespace GestionActivos.Application.ActivoApplication.Queries
{
    public record GetActivoByIdQuery(Guid Id) : IRequest<ActivoDto>;

    public class GetActivoByIdHandler : IRequestHandler<GetActivoByIdQuery, ActivoDto>
    {
        private readonly IActivoRepository _activoRepository;
        private readonly IMapper _mapper;

        public GetActivoByIdHandler(IActivoRepository activoRepository, IMapper mapper)
        {
            _activoRepository = activoRepository;
            _mapper = mapper;
        }

        public async Task<ActivoDto> Handle(
            GetActivoByIdQuery request,
            CancellationToken cancellationToken
        )
        {
            var activo = await _activoRepository.GetByIdAsync(request.Id);

            if (activo == null)
            {
                throw new NotFoundException($"No se encontró el activo con ID {request.Id}.");
            }

            return _mapper.Map<ActivoDto>(activo);
        }
    }
}
