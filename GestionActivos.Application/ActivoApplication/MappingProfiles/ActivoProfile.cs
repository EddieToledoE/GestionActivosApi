using AutoMapper;
using GestionActivos.Application.ActivoApplication.DTOs;
using GestionActivos.Domain.Entities;

namespace GestionActivos.Application.ActivoApplication.MappingProfiles
{
    public class ActivoProfile : Profile
    {
        public ActivoProfile()
        {
   // Mapeo de CreateActivoDto a Activo (sin los archivos, se manejan aparte)
      CreateMap<CreateActivoDto, Activo>()
     .ForMember(dest => dest.IdActivo, opt => opt.Ignore())
      .ForMember(dest => dest.ImagenUrl, opt => opt.Ignore())
 .ForMember(dest => dest.FacturaPDF, opt => opt.Ignore())
    .ForMember(dest => dest.FacturaXML, opt => opt.Ignore())
     .ForMember(dest => dest.Estatus, opt => opt.MapFrom(src => "Activo"))
      .ForMember(dest => dest.FechaAlta, opt => opt.MapFrom(src => DateTime.Now))
 .ForMember(dest => dest.Responsable, opt => opt.Ignore())
            .ForMember(dest => dest.CategoriaNavigation, opt => opt.Ignore())
      .ForMember(dest => dest.Reubicaciones, opt => opt.Ignore())
      .ForMember(dest => dest.Diagnosticos, opt => opt.Ignore())
       .ForMember(dest => dest.DetallesAuditoria, opt => opt.Ignore());

 // Mapeo de Activo a ActivoDto
      CreateMap<Activo, ActivoDto>();
     }
    }
}
