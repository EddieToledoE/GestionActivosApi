using GestionActivos.Application.UsuarioApplication.DTOs;
using GestionActivos.Domain.Exceptions;
using GestionActivos.Domain.Interfaces;
using MediatR;

namespace GestionActivos.Application.UsuarioApplication.Queries
{
    public record GetUsuarioByClaveFortiaQuery(string ClaveFortia) : IRequest<UsuarioCentroCostoDto>;

    public class GetUsuarioByClaveFortiaHandler : IRequestHandler<GetUsuarioByClaveFortiaQuery, UsuarioCentroCostoDto>
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public GetUsuarioByClaveFortiaHandler(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<UsuarioCentroCostoDto> Handle(
            GetUsuarioByClaveFortiaQuery request,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.ClaveFortia))
            {
                throw new ArgumentException("La clave Fortia no puede estar vacía.", nameof(request.ClaveFortia));
            }

            var usuario = await _usuarioRepository.GetByClaveFortiaAsync(request.ClaveFortia);

            if (usuario == null)
            {
                throw new NotFoundException($"No se encontró ningún usuario con la clave Fortia '{request.ClaveFortia}'.");
            }

            // Construir nombre completo
            var nombreCompleto = $"{usuario.Nombres} {usuario.ApellidoPaterno}";
            if (!string.IsNullOrWhiteSpace(usuario.ApellidoMaterno))
            {
                nombreCompleto += $" {usuario.ApellidoMaterno}";
            }

            return new UsuarioCentroCostoDto
            {
                IdUsuario = usuario.IdUsuario,
                NombreCompleto = nombreCompleto,
                ClaveFortia = usuario.ClaveFortia,
                IdCentroCosto = usuario.IdCentroCosto,
                Ubicacion = usuario.CentroCosto?.Ubicacion,
                RazonSocial = usuario.CentroCosto?.RazonSocial,
                Area = usuario.CentroCosto?.Area
            };
        }
    }
}
