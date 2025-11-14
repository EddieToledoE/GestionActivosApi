using System.Net;
using System.Text.Json;
using GestionActivos.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionActivos.API.Middleware
{
    /// <summary>
    /// Middleware para manejo centralizado de excepciones usando ProblemDetails (RFC 7807).
    /// </summary>
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
            var problemDetails = CreateProblemDetails(context, exception);

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = problemDetails.Status ?? (int)HttpStatusCode.InternalServerError;

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            var result = JsonSerializer.Serialize(problemDetails, options);
            return context.Response.WriteAsync(result);
        }

        private static ProblemDetails CreateProblemDetails(HttpContext context, Exception exception)
        {
            var statusCode = HttpStatusCode.InternalServerError;
            var title = "Error interno del servidor";
            var detail = "Ocurrió un error inesperado. Por favor, contacte al administrador.";
            var type = "https://tools.ietf.org/html/rfc7231#section-6.6.1";
            Dictionary<string, object?>? extensions = null;

            switch (exception)
            {
                case ValidationException validationException:
                    statusCode = HttpStatusCode.BadRequest;
                    title = "Error de validación";
                    detail = validationException.Message;
                    type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
                    extensions = new Dictionary<string, object?>
                    {
                        { "errors", validationException.Errors }
                    };
                    break;

                case NotFoundException notFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    title = "Recurso no encontrado";
                    detail = notFoundException.Message;
                    type = "https://tools.ietf.org/html/rfc7231#section-6.5.4";
                    break;

                case BusinessException businessException:
                    statusCode = HttpStatusCode.BadRequest;
                    title = "Error de regla de negocio";
                    detail = businessException.Message;
                    type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
                    break;

                case ArgumentNullException argumentNullException:
                    statusCode = HttpStatusCode.BadRequest;
                    title = "Argumento nulo o inválido";
                    detail = argumentNullException.Message;
                    type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
                    extensions = new Dictionary<string, object?>
                    {
                        { "paramName", argumentNullException.ParamName }
                    };
                    break;

                case ArgumentException argumentException:
                    statusCode = HttpStatusCode.BadRequest;
                    title = "Argumento inválido";
                    detail = argumentException.Message;
                    type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
                    extensions = new Dictionary<string, object?>
                    {
                        { "paramName", argumentException.ParamName }
                    };
                    break;

                case DbUpdateException dbUpdateException:
                    statusCode = HttpStatusCode.BadRequest;
                    title = "Error de base de datos";
                    detail = ParseDatabaseError(dbUpdateException);
                    type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
                    break;

                case UnauthorizedAccessException:
                    statusCode = HttpStatusCode.Unauthorized;
                    title = "No autorizado";
                    detail = "No tiene permisos para realizar esta operación.";
                    type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
                    break;

                default:
                    // Para errores 500, no exponer detalles internos en producción
                    statusCode = HttpStatusCode.InternalServerError;
                    title = "Error interno del servidor";
                    detail = "Ocurrió un error inesperado. Por favor, contacte al administrador.";
                    type = "https://tools.ietf.org/html/rfc7231#section-6.6.1";
                    extensions = new Dictionary<string, object?>
                    {
                        { "exceptionType", exception.GetType().Name }
                    };
                    break;
            }

            var problemDetails = new ProblemDetails
            {
                Status = (int)statusCode,
                Title = title,
                Detail = detail,
                Type = type,
                Instance = context.Request.Path
            };

            // Agregar extensiones si existen
            if (extensions != null)
            {
                foreach (var extension in extensions)
                {
                    problemDetails.Extensions[extension.Key] = extension.Value;
                }
            }

            // Agregar TraceId para debugging
            problemDetails.Extensions["traceId"] = context.TraceIdentifier;

            return problemDetails;
        }

        private static string ParseDatabaseError(DbUpdateException dbUpdateException)
        {
            var errorMessage = "Error al actualizar la base de datos.";
            var innerException = dbUpdateException.InnerException;

            if (innerException != null)
            {
                var innerMessage = innerException.Message;

                // Verificar errores comunes de SQL Server
                if (innerMessage.Contains("UNIQUE KEY") || 
                    innerMessage.Contains("IX_") || 
                    innerMessage.Contains("Cannot insert duplicate key"))
                {
                    errorMessage = "Ya existe un registro con los mismos valores únicos.";
                }
                else if (innerMessage.Contains("FOREIGN KEY") || innerMessage.Contains("FK_"))
                {
                    errorMessage = "No se puede realizar la operación debido a una restricción de integridad referencial.";
                }
                else if (innerMessage.Contains("PRIMARY KEY") || innerMessage.Contains("PK_"))
                {
                    errorMessage = "Violación de clave primaria. El registro ya existe.";
                }
                else if (innerMessage.Contains("CHECK constraint"))
                {
                    errorMessage = "Los datos no cumplen con las restricciones de validación.";
                }
                else if (innerMessage.Contains("DELETE") && innerMessage.Contains("REFERENCE constraint"))
                {
                    errorMessage = "No se puede eliminar el registro porque está siendo referenciado por otros datos.";
                }
            }

            return errorMessage;
        }
    }
}

