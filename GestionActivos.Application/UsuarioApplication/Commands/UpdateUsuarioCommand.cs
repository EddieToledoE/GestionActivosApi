using AutoMapper;
using GestionActivos.Application.UsuarioApplication.DTOs;
using GestionActivos.Domain.Interfaces;
using MediatR;

namespace GestionActivos.Application.UsuarioApplication.Commands
{
    public record UpdateUsuarioCommand(UsuarioDto Usuario) : IRequest<bool>;

    public class UpdateUsuarioHandler : IRequestHandler<UpdateUsuarioCommand, bool>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;

        public UpdateUsuarioHandler(IUsuarioRepository usuarioRepository, IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(
            UpdateUsuarioCommand request,
            CancellationToken cancellationToken
        )
        {
            var entity = _mapper.Map<Domain.Entities.Usuario>(request.Usuario);
            await _usuarioRepository.UpdateAsync(entity);
            return true;
        }
    }
}
