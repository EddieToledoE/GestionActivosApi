using GestionActivos.Domain.Entities;
using GestionActivos.Domain.Exceptions;
using GestionActivos.Domain.Interfaces;
using MediatR;

namespace GestionActivos.Application.AuthApplication.Commands
{
    public record LoginCommand(string Correo, string Contrasena) : IRequest<Usuario>;

    public class LoginHandler : IRequestHandler<LoginCommand, Usuario>
    {
        private readonly IAuthRepository _authRepository;

        public LoginHandler(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<Usuario> Handle(
            LoginCommand request,
            CancellationToken cancellationToken
        )
        {
            if (string.IsNullOrWhiteSpace(request.Correo))
            {
                throw new BusinessException("El correo electrónico es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(request.Contrasena))
            {
                throw new BusinessException("La contraseña es obligatoria.");
            }

            var usuario = await _authRepository.LoginAsync(request.Correo, request.Contrasena);

            if (usuario == null)
            {
                throw new BusinessException("Credenciales incorrectas.");
            }

            if (!usuario.Activo)
            {
                throw new BusinessException("El usuario está desactivado.");
            }

            return usuario;
        }
    }
}
