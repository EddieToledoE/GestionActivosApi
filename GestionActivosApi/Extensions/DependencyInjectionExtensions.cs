using GestionActivos.Domain.Interfaces;
using GestionActivos.Infrastructure.Repositories;
using GestionActivos.Infrastructure.Services;

namespace GestionActivos.API.Extensions
{
    internal static class DependencyInjectionExtensions
    {
        internal static IServiceCollection AddDependencyInjectionRepositories(
            this IServiceCollection services
        )
        {
            // Repositorios
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<ICategoriaRepository, CategoriaRepository>();
            services.AddScoped<IActivoRepository, ActivoRepository>();
            services.AddScoped<ISolicitudRepository, SolicitudRepository>();

            return services;
        }

        internal static IServiceCollection AddDependencyInjectionUnitsOfWork(
            this IServiceCollection services
        )
        {
            // (En el futuro: registrar UnitOfWork)
            return services;
        }

        internal static IServiceCollection AddDependencyInjectionServices(
            this IServiceCollection services
        )
        {
            // Servicios
            services.AddScoped<IFileStorageService, MinioStorageService>();

            return services;
        }
    }
}
