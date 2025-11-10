using System.Reflection;
using FluentValidation;
using GestionActivos.Application.Behaviors;
using MediatR;

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
                // Registrar el comportamiento de validación
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            // Registrar todos los validadores de FluentValidation
            services.AddValidatorsFromAssembly(applicationAssembly);

            return services;
        }
    }
}
