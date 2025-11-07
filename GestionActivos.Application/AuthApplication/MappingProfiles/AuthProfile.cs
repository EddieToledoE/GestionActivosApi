using AutoMapper;
using GestionActivos.Application.AuthApplication.DTOs;
using GestionActivos.Domain.Entities;

namespace GestionActivos.Application.AuthApplication.MappingProfiles
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<LoginDto, Usuario>();
        }
    }
}
