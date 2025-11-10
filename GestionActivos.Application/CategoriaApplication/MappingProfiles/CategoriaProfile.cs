using AutoMapper;
using GestionActivos.Application.CategoriaApplication.DTOs;
using GestionActivos.Domain.Entities;

namespace GestionActivos.Application.CategoriaApplication.MappingProfiles
{
    public class CategoriaProfile : Profile
    {
        public CategoriaProfile()
        {
     CreateMap<Categoria, CategoriaDto>();
            CreateMap<CategoriaDto, Categoria>();
        }
    }
}
