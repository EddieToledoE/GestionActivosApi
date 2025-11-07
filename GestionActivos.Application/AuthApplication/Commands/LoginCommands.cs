using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionActivos.Domain.Entities;
using GestionActivos.Domain.Interfaces;
using MediatR;

namespace GestionActivos.Application.AuthApplication.Commands
{
    public record LoginCommand(string Correo, string Contrasena) : IRequest<Usuario?>;

    public class LoginHandler : IRequestHandler<LoginCommand, Usuario?>
    {
        private readonly IAuthRepository _authRepository;

        public LoginHandler(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<Usuario?> Handle(
            LoginCommand request,
            CancellationToken cancellationToken
        )
        {
            return await _authRepository.LoginAsync(request.Correo, request.Contrasena);
        }
    }
}
