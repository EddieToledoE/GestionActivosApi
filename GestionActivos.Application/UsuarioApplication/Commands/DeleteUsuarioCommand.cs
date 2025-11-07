using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionActivos.Domain.Interfaces;
using MediatR;

namespace GestionActivos.Application.UsuarioApplication.Commands
{
    public record DeleteUsuarioCommand(int IdUsuario) : IRequest<bool>;

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
            var user = await _usuarioRepository.GetByIdAsync(request.IdUsuario);
            if (user is null)
                return false;

            user.Activo = false;
            await _usuarioRepository.UpdateAsync(user);
            return true;
        }
    }
}
