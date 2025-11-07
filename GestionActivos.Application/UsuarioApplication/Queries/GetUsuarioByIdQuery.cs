using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GestionActivos.Application.UsuarioApplication.DTOs;
using GestionActivos.Domain.Interfaces;
using MediatR;

namespace GestionActivos.Application.UsuarioApplication.Queries
{
    public record GetUsuarioByIdQuery(int IdUsuario) : IRequest<UsuarioDto?>;

    public class GetUsuarioByIdHandler : IRequestHandler<GetUsuarioByIdQuery, UsuarioDto?>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;

        public GetUsuarioByIdHandler(IUsuarioRepository usuarioRepository, IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
        }

        public async Task<UsuarioDto?> Handle(
            GetUsuarioByIdQuery request,
            CancellationToken cancellationToken
        )
        {
            var usuario = await _usuarioRepository.GetByIdAsync(request.IdUsuario);
            return _mapper.Map<UsuarioDto?>(usuario);
        }
    }
}
