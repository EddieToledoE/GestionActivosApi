using System.Net;
using System.Text.Json;
using GestionActivos.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace GestionActivos.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger
        )
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió una excepción no manejada: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            var result = string.Empty;

            switch (exception)
            {
                case ValidationException validationException:
                    code = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(
                        new
                        {
                            error = "Error de validación",
                            message = validationException.Message,
                            errors = validationException.Errors,
                        }
                    );
                    break;

                case NotFoundException notFoundException:
                    code = HttpStatusCode.NotFound;
                    result = JsonSerializer.Serialize(
                        new { error = "No encontrado", message = notFoundException.Message }
                    );
                    break;

                case BusinessException businessException:
                    code = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(
                        new { error = "Error de negocio", message = businessException.Message }
                    );
                    break;

                case ArgumentNullException argumentNullException:
                    code = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(
                        new
                        {
                            error = "Argumento inválido",
                            message = argumentNullException.Message,
                        }
                    );
                    break;

                case DbUpdateException dbUpdateException:
                    code = HttpStatusCode.BadRequest;
                    var errorMessage = "Error al actualizar la base de datos.";
                    var innerException = dbUpdateException.InnerException;

                    if (innerException != null)
                    {
                        var innerMessage = innerException.Message;

                        // Verificar errores de SQL Server
                        if (innerMessage.Contains("UNIQUE KEY") || innerMessage.Contains("IX_") || innerMessage.Contains("Cannot insert duplicate key"))
                        {
                            errorMessage = "Ya existe un registro con los mismos valores únicos.";
                        }
                        else if (innerMessage.Contains("FOREIGN KEY") || innerMessage.Contains("FK_"))
                        {
                            errorMessage = "No se puede realizar la operación debido a una restricción de clave foránea.";
                        }
                        else if (innerMessage.Contains("PRIMARY KEY") || innerMessage.Contains("PK_"))
                        {
                            errorMessage = "Violación de clave primaria.";
                        }
                        else if (innerMessage.Contains("CHECK constraint"))
                        {
                            errorMessage = "Los datos no cumplen con las restricciones de validación.";
                        }
                    }

                    result = JsonSerializer.Serialize(
                        new { error = "Error de base de datos", message = errorMessage }
                    );
                    break;

                default:
                    result = JsonSerializer.Serialize(
                        new
                        {
                            error = "Error interno del servidor",
                            message = "Ocurrió un error inesperado. Por favor, contacte al administrador.",
                        }
                    );
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(result);
        }
    }
}

