using AutoMapper;
using GestionActivos.Application.UsuarioApplication.DTOs;
using GestionActivos.Domain.Entities;

namespace GestionActivos.Application.UsuarioApplication.MappingProfiles
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            CreateMap<Usuario, UsuarioDto>();
            CreateMap<CreateUsuarioDto, Usuario>();
            
            // Mapeo para UpdateUsuarioDto - se maneja manualmente en el handler
            // No se crea mapeo automático para evitar actualizar campos no deseados
        }
    }
}
