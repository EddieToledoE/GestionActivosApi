using AutoMapper;
using GestionActivos.Application.UsuarioApplication.DTOs;
using GestionActivos.Domain.Exceptions;
using GestionActivos.Domain.Interfaces;
using MediatR;

namespace GestionActivos.Application.UsuarioApplication.Queries
{
    public record GetUsuarioByIdQuery(int IdUsuario) : IRequest<UsuarioDto>;

    public class GetUsuarioByIdHandler : IRequestHandler<GetUsuarioByIdQuery, UsuarioDto>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;

        public GetUsuarioByIdHandler(IUsuarioRepository usuarioRepository, IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
        }

        public async Task<UsuarioDto> Handle(
            GetUsuarioByIdQuery request,
            CancellationToken cancellationToken
        )
        {
            if (request.IdUsuario <= 0)
            {
                throw new BusinessException("El ID del usuario debe ser mayor que 0.");
            }

            var usuario = await _usuarioRepository.GetByIdAsync(request.IdUsuario);
            if (usuario == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Usuario), request.IdUsuario);
            }

            return _mapper.Map<UsuarioDto>(usuario);
        }
    }
}
