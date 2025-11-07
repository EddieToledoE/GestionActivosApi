using System.Reflection;

namespace GestionActivos.API.Extensions
{
    internal static class MediatRServiceExtensions
    {
        internal static IServiceCollection AddMediatRSettings(this IServiceCollection services)
        {
            Assembly? applicationAssembly = Assembly.Load("GestionActivos.Application");

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(applicationAssembly);
            });

            return services;
        }
    }
}
