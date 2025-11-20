using GestionActivos.API.Extensions;
using GestionActivos.API.Middleware;
using GestionActivos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// === CONFIGURACIÓN ===
ConfigureAppConfiguration(builder);
ConfigureLogging(builder);
ConfigureServices(builder);

WebApplication app = builder.Build();

// === CREAR O MIGRAR LA BASE DE DATOS AUTOMÁTICAMENTE ===
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // ✅ Si es entorno de desarrollo: crea la BD si no existe
    // ✅ Si es entorno productivo: aplica migraciones existentes
    if (app.Environment.IsDevelopment())
    {
        dbContext.Database.EnsureCreated();
        Console.WriteLine("✔ Base de datos creada automáticamente (EnsureCreated)");
    }
    else
    {
        dbContext.Database.Migrate();
        Console.WriteLine("✔ Migraciones aplicadas automáticamente (Migrate)");
    }
}

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
    // Middleware de manejo de excepciones debe ir primero
    app.UseMiddleware<ExceptionHandlingMiddleware>();

    if (env.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
}
