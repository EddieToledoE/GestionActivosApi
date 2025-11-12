using AutoMapper;
using GestionActivos.Application.SolicitudApplication.DTOs;
using GestionActivos.Domain.Entities;

namespace GestionActivos.Application.SolicitudApplication.MappingProfiles
{
    public class SolicitudProfile : Profile
    {
        public SolicitudProfile()
        {
            // Mapeo de CreateSolicitudDto a Solicitud
            CreateMap<CreateSolicitudDto, Solicitud>()
                .ForMember(dest => dest.IdSolicitud, opt => opt.Ignore())
                .ForMember(dest => dest.Fecha, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => "Pendiente"))
                .ForMember(dest => dest.Emisor, opt => opt.Ignore())
                .ForMember(dest => dest.Receptor, opt => opt.Ignore())
                .ForMember(dest => dest.Activo, opt => opt.Ignore());

            // Mapeo de Solicitud a SolicitudDto
            CreateMap<Solicitud, SolicitudDto>()
                .ForMember(dest => dest.NombreEmisor, opt => opt.MapFrom(src => 
                    $"{src.Emisor.Nombres} {src.Emisor.ApellidoPaterno}" + 
                    (string.IsNullOrWhiteSpace(src.Emisor.ApellidoMaterno) ? "" : $" {src.Emisor.ApellidoMaterno}")))
                .ForMember(dest => dest.NombreReceptor, opt => opt.MapFrom(src => 
                    $"{src.Receptor.Nombres} {src.Receptor.ApellidoPaterno}" + 
                    (string.IsNullOrWhiteSpace(src.Receptor.ApellidoMaterno) ? "" : $" {src.Receptor.ApellidoMaterno}")))
                .ForMember(dest => dest.EtiquetaActivo, opt => opt.MapFrom(src => src.Activo.Etiqueta))
                .ForMember(dest => dest.DescripcionActivo, opt => opt.MapFrom(src => src.Activo.Descripcion));
        }
    }
}
