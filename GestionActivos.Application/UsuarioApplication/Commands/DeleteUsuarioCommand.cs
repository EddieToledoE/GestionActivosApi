using GestionActivos.Domain.Exceptions;
using GestionActivos.Domain.Interfaces;
using MediatR;

namespace GestionActivos.Application.UsuarioApplication.Commands
{
    public record DeleteUsuarioCommand(Guid IdUsuario) : IRequest<bool>;

    public class DeleteUsuarioHandler : IRequestHandler<DeleteUsuarioCommand, bool>
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public DeleteUsuarioHandler(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<bool> Handle(
            DeleteUsuarioCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request.IdUsuario == Guid.Empty)
            {
                throw new BusinessException("El ID del usuario no puede ser vacío.");
            }

            var user = await _usuarioRepository.GetByIdAsync(request.IdUsuario);
            if (user is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Usuario), request.IdUsuario);
            }

            if (!user.Activo)
            {
                throw new BusinessException("El usuario ya está desactivado.");
            }

            user.Activo = false;
            await _usuarioRepository.UpdateAsync(user);
            return true;
        }
    }
}
