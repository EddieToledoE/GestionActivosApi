using GestionActivos.Application.AuthApplication.DTOs;
using GestionActivos.Domain.Exceptions;
using GestionActivos.Domain.Interfaces;
using MediatR;

namespace GestionActivos.Application.AuthApplication.Commands
{
    public record LoginCommand(string Correo, string Contrasena) : IRequest<LoginResponseDto>;

    public class LoginHandler : IRequestHandler<LoginCommand, LoginResponseDto>
    {
        private readonly IAuthRepository _authRepository;

        public LoginHandler(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<LoginResponseDto> Handle(
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

            // Mapear a LoginResponseDto
            var response = new LoginResponseDto
            {
                IdUsuario = usuario.IdUsuario,
                Nombres = usuario.Nombres,
                ApellidoPaterno = usuario.ApellidoPaterno,
                ApellidoMaterno = usuario.ApellidoMaterno,
                Correo = usuario.Correo,
                ClaveFortia = usuario.ClaveFortia,

                // Centro de costo principal (legacy)
                IdCentroCostoPrincipal = usuario.IdCentroCosto,
                CentroCostoPrincipal = usuario.CentroCosto != null
                    ? $"{usuario.CentroCosto.RazonSocial} - {usuario.CentroCosto.Ubicacion}".Trim()
                    : null,

                // Roles con sus permisos
                Roles = usuario.Roles.Select(ur => new RolInfoDto
                {
                    IdRol = ur.Rol.IdRol,
                    Nombre = ur.Rol.Nombre,
                    Permisos = ur.Rol.Permisos.Select(rp => rp.Permiso.Nombre).ToList()
                }).ToList(),

                // Lista plana de permisos únicos (para control de UI)
                Permisos = usuario.Roles
                    .SelectMany(ur => ur.Rol.Permisos.Select(rp => rp.Permiso.Nombre))
                    .Distinct()
                    .OrderBy(p => p)
                    .ToList(),

                // Centros de costo con acceso (para control de backend)
                CentrosCostoAcceso = usuario.UsuarioCentrosCosto
                    .Where(ucc => ucc.Activo) // Solo los activos
                    .Select(ucc => new CentroCostoAccessDto
                    {
                        IdCentroCosto = ucc.IdCentroCosto,
                        RazonSocial = ucc.CentroCosto.RazonSocial,
                        Ubicacion = ucc.CentroCosto.Ubicacion,
                        Area = ucc.CentroCosto.Area,
                        Activo = ucc.CentroCosto.Activo
                    })
                    .ToList()
            };

            return response;
        }
    }
}
