using AutoMapper;
using GestionActivos.Application.UsuarioApplication.DTOs;
using GestionActivos.Domain.Entities;
using GestionActivos.Domain.Exceptions;
using GestionActivos.Domain.Interfaces;
using MediatR;
using System;

namespace GestionActivos.Application.UsuarioApplication.Commands
{
    public record CreateUsuarioCommand(CreateUsuarioDto Usuario) : IRequest<Guid>;

    public class CreateUsuarioHandler : IRequestHandler<CreateUsuarioCommand, Guid>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;

        public CreateUsuarioHandler(IUsuarioRepository usuarioRepository, IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(
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

            // Validar si el correo ya existe
            if (!string.IsNullOrEmpty(request.Usuario.Correo))
            {
                var existeCorreo = await _usuarioRepository.ExistsByCorreoAsync(
                    request.Usuario.Correo
                );
                if (existeCorreo)
                {
                    throw new BusinessException(
                        $"Ya existe un usuario con el correo '{request.Usuario.Correo}'."
                    );
                }
            }

            var usuario = _mapper.Map<Usuario>(request.Usuario);

            await _usuarioRepository.AddAsync(usuario);
            return usuario.IdUsuario;
        }
    }
}
