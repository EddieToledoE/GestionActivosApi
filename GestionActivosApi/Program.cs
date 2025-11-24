using GestionActivos.API.Extensions;
using GestionActivos.API.Filters;
using GestionActivos.API.Middleware;
using GestionActivos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
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
    
    // Configuración de Swagger con soporte para headers personalizados
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Gestión de Activos API",
            Version = "v1",
            Description = "API para la gestión de activos empresariales con control de permisos y centros de costo",
            Contact = new OpenApiContact
            {
                Name = "Equipo de Desarrollo",
                Email = "desarrollo@empresa.com"
            }
        });

        // Agregar filtro para headers personalizados (X-User-Id)
        options.OperationFilter<AddUserIdHeaderOperationFilter>();

        // Habilitar comentarios XML si existen
        var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        if (File.Exists(xmlPath))
        {
            options.IncludeXmlComments(xmlPath);
        }
    });
}

void ConfigurePipeline(WebApplication app, IHostEnvironment env)
{
    // Middleware de manejo de excepciones debe ir primero
    app.UseMiddleware<ExceptionHandlingMiddleware>();

    if (env.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Gestión de Activos API v1");
            options.RoutePrefix = "swagger";
            options.DocumentTitle = "Gestión de Activos API";
        });
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
}
