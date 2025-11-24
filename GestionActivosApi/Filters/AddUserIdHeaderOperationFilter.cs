using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GestionActivos.API.Filters
{
    /// <summary>
    /// Filtro de operación de Swagger para agregar el header X-User-Id
    /// a todos los endpoints que lo requieran.
    /// </summary>
    public class AddUserIdHeaderOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= new List<OpenApiParameter>();

            // Agregar header X-User-Id a todos los endpoints
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "X-User-Id",
                In = ParameterLocation.Header,
                Required = false,
                Description = "ID del usuario autenticado (GUID)",
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Format = "uuid"
                }
            });
        }
    }
}
