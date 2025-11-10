using GestionActivos.Application.UsuarioApplication.DTOs;
using GestionActivos.Domain.Entities;
using GestionActivos.Domain.Exceptions;
using GestionActivos.Domain.Interfaces;
using MediatR;

namespace GestionActivos.Application.UsuarioApplication.Commands
{
    public record UpdateUsuarioCommand(UpdateUsuarioDto Usuario) : IRequest<bool>;

    public class UpdateUsuarioHandler : IRequestHandler<UpdateUsuarioCommand, bool>
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UpdateUsuarioHandler(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
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
                throw new NotFoundException(nameof(Usuario), request.Usuario.Id);
            }

            // Validar si el correo ya existe en otro usuario
            if (
                !string.IsNullOrEmpty(request.Usuario.Correo)
                && request.Usuario.Correo != usuario.Correo
            )
            {
                var existeCorreo = await _usuarioRepository.ExistsByCorreoAsync(
                    request.Usuario.Correo
                );
                if (existeCorreo)
                {
                    throw new BusinessException(
                        $"Ya existe otro usuario con el correo '{request.Usuario.Correo}'."
                    );
                }
            }

            // Actualizar solo los campos que no son null o vacíos
            if (!string.IsNullOrWhiteSpace(request.Usuario.Nombres))
            {
                usuario.Nombres = request.Usuario.Nombres;
            }

            if (!string.IsNullOrWhiteSpace(request.Usuario.ApellidoPaterno))
            {
                usuario.ApellidoPaterno = request.Usuario.ApellidoPaterno;
            }

            if (!string.IsNullOrWhiteSpace(request.Usuario.ApellidoMaterno))
            {
                usuario.ApellidoMaterno = request.Usuario.ApellidoMaterno;
            }

            if (!string.IsNullOrWhiteSpace(request.Usuario.Correo))
            {
                usuario.Correo = request.Usuario.Correo;
            }

            if (!string.IsNullOrWhiteSpace(request.Usuario.Contrasena))
            {
                usuario.Contrasena = request.Usuario.Contrasena;
            }

            await _usuarioRepository.UpdateAsync(usuario);

            return true;
        }
    }
}
