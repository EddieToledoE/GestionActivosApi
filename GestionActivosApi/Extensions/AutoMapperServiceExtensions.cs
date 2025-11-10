using AutoMapper;
using GestionActivos.Application.ActivoApplication.MappingProfiles;
using GestionActivos.Application.AuthApplication.MappingProfiles;
using GestionActivos.Application.CategoriaApplication.MappingProfiles;
using GestionActivos.Application.UsuarioApplication.MappingProfiles;

namespace GestionActivos.API.Extensions
{
    internal static class AutoMapperServiceExtensions
    {
        internal static IServiceCollection AddAutoMapperProfiles(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<UsuarioProfile>();
                cfg.AddProfile<AuthProfile>();
                cfg.AddProfile<CategoriaProfile>();
                cfg.AddProfile<ActivoProfile>();
            });

            return services;
        }
    }
}
