using AutoMapper;
using GestionActivos.Application.CategoriaApplication.DTOs;
using GestionActivos.Domain.Interfaces;
using MediatR;

namespace GestionActivos.Application.CategoriaApplication.Queries
{
    public record GetCategoriasQuery : IRequest<IEnumerable<CategoriaDto>>;

    public class GetCategoriasHandler : IRequestHandler<GetCategoriasQuery, IEnumerable<CategoriaDto>>
    {
        private readonly ICategoriaRepository _categoriaRepository;
    private readonly IMapper _mapper;

        public GetCategoriasHandler(ICategoriaRepository categoriaRepository, IMapper mapper)
        {
            _categoriaRepository = categoriaRepository;
            _mapper = mapper;
   }

  public async Task<IEnumerable<CategoriaDto>> Handle(
  GetCategoriasQuery request,
            CancellationToken cancellationToken
        )
        {
      var categorias = await _categoriaRepository.GetAllAsync();
   return _mapper.Map<IEnumerable<CategoriaDto>>(categorias);
        }
    }
}
