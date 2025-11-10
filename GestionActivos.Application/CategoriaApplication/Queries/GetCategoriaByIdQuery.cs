using AutoMapper;
using GestionActivos.Application.CategoriaApplication.DTOs;
using GestionActivos.Domain.Exceptions;
using GestionActivos.Domain.Interfaces;
using MediatR;

namespace GestionActivos.Application.CategoriaApplication.Queries
{
    public record GetCategoriaByIdQuery(int IdCategoria) : IRequest<CategoriaDto>;

    public class GetCategoriaByIdHandler : IRequestHandler<GetCategoriaByIdQuery, CategoriaDto>
    {
        private readonly ICategoriaRepository _categoriaRepository;
private readonly IMapper _mapper;

        public GetCategoriaByIdHandler(ICategoriaRepository categoriaRepository, IMapper mapper)
        {
     _categoriaRepository = categoriaRepository;
  _mapper = mapper;
        }

        public async Task<CategoriaDto> Handle(
    GetCategoriaByIdQuery request,
            CancellationToken cancellationToken
        )
    {
            if (request.IdCategoria <= 0)
            {
          throw new BusinessException("El ID de la categoría debe ser mayor que 0.");
   }

  var categoria = await _categoriaRepository.GetByIdAsync(request.IdCategoria);
            if (categoria == null)
            {
  throw new NotFoundException(nameof(Domain.Entities.Categoria), request.IdCategoria);
    }

            return _mapper.Map<CategoriaDto>(categoria);
        }
    }
}
