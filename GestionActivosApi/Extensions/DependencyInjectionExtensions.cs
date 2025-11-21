using GestionActivos.Application.AuditoriaApplication.Services;
using GestionActivos.Domain.Interfaces;
using GestionActivos.Domain.Interfaces.UnitsOfWork;
using GestionActivos.Infrastructure.Repositories;
using GestionActivos.Infrastructure.Services;
using GestionActivos.Infrastructure.UnitsOfWork;

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
            services.AddScoped<IReubicacionRepository, ReubicacionRepository>();
            services.AddScoped<INotificacionRepository, NotificacionRepository>();
            services.AddScoped<IConfigAuditoriaRepository, ConfigAuditoriaRepository>();
            services.AddScoped<IAuditoriaRepository, AuditoriaRepository>();
            services.AddScoped<IUsuarioCentroCostoRepository, UsuarioCentroCostoRepository>();

            return services;
        }

        internal static IServiceCollection AddDependencyInjectionUnitsOfWork(
            this IServiceCollection services
        )
        {
            // Units of Work por Contexto/Agregado
            services.AddScoped<ISolicitudUnitOfWork, SolicitudUnitOfWork>();
            services.AddScoped<IAuditoriaUnitOfWork, AuditoriaUnitOfWork>();
            services.AddScoped<ITransferenciaUnitOfWork, TransferenciaUnitOfWork>();

            return services;
        }

        internal static IServiceCollection AddDependencyInjectionServices(
            this IServiceCollection services
        )
        {
            // Servicios
            services.AddScoped<IFileStorageService, MinioStorageService>();
            services.AddScoped<AuditoriaService>();

            return services;
        }
    }
}
