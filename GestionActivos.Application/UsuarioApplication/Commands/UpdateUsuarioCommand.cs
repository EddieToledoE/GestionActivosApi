using AutoMapper;
using GestionActivos.Application.UsuarioApplication.DTOs;
using GestionActivos.Domain.Entities;
using GestionActivos.Domain.Interfaces;
using MediatR;

namespace GestionActivos.Application.UsuarioApplication.Commands
{
    public record UpdateUsuarioCommand(UpdateUsuarioDto Usuario) : IRequest<bool>;

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
            if (request.Usuario == null)
            {
                throw new ArgumentNullException(
                    nameof(request.Usuario),
                    "El usuario no puede ser nulo."
                );
            }

            Usuario? usuario = await _usuarioRepository.GetByIdAsync(request.Usuario.Id);
            if (usuario == null)
            {
                return false;
            }

            // Mapear los campos del DTO sobre la entidad existente
            await _usuarioRepository.UpdateAsync(_mapper.Map(request.Usuario, usuario));

            return true;
        }
    }
}
