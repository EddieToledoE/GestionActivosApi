using AutoMapper;
using GestionActivos.Application.UsuarioApplication.DTOs;
using GestionActivos.Domain.Entities;
using GestionActivos.Domain.Interfaces;
using MediatR;

namespace GestionActivos.Application.UsuarioApplication.Commands
{
    public record CreateUsuarioCommand(CreateUsuarioDto Usuario) : IRequest<int>;

    public class CreateUsuarioHandler : IRequestHandler<CreateUsuarioCommand, int>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;

        public CreateUsuarioHandler(IUsuarioRepository usuarioRepository, IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
        }

        public async Task<int> Handle(
            CreateUsuarioCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request.Usuario == null)
            {
                throw new ArgumentNullException(
                    nameof(request.Usuario),
                    "El usuario no puede ser nulo."
                );
            }
            var usuario = _mapper.Map<Usuario>(request.Usuario);

            await _usuarioRepository.AddAsync(usuario);
            return usuario.IdUsuario;
        }
    }
}
