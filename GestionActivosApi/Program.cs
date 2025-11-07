using GestionActivos.API.Extensions;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// === CONFIGURACIÓN ===
ConfigureAppConfiguration(builder);
ConfigureLogging(builder);
ConfigureServices(builder);

WebApplication app = builder.Build();

// === PIPELINE HTTP ===
ConfigurePipeline(app, app.Environment);

app.Run();

// ============================ MÉTODOS ============================

void ConfigureAppConfiguration(WebApplicationBuilder builder)
{
    builder.Configuration.AddEnvironmentVariables();
}

void ConfigureLogging(WebApplicationBuilder builder)
{
    builder.Host.UseSerilog(
        (context, config) =>
        {
            config.MinimumLevel.Information();
            config.WriteTo.Console();
            config.WriteTo.File("logs/app.log", rollingInterval: RollingInterval.Day);
        }
    );
}

void ConfigureServices(WebApplicationBuilder builder)
{
    // Base de datos
    builder.Services.AddDbConnection(builder.Configuration);

    // Dependencias
    builder.Services.AddDependencyInjectionRepositories();
    builder.Services.AddDependencyInjectionUnitsOfWork();
    builder.Services.AddDependencyInjectionServices();

    // MediatR y AutoMapper
    builder.Services.AddMediatRSettings();
    builder.Services.AddAutoMapperProfiles();

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

void ConfigurePipeline(WebApplication app, IHostEnvironment env)
{
    if (env.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
}
