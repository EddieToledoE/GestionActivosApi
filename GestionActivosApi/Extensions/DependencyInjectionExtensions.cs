using GestionActivos.Domain.Interfaces;
using GestionActivos.Infrastructure.Repositories;

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
            // (En el futuro: registrar servicios adicionales)
            return services;
        }
    }
}
