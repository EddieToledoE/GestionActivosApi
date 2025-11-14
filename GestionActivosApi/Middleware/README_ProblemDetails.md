# Implementación de ProblemDetails (RFC 7807) para Manejo de Errores

## Descripción
Se ha refactorizado el middleware de manejo de excepciones para usar **ProblemDetails**, que es el estándar RFC 7807 de ASP.NET Core para representar errores en APIs REST.

---

## ¿Qué es ProblemDetails?

`ProblemDetails` es una especificación estándar (RFC 7807) que define un formato JSON consistente para reportar errores en APIs HTTP. Proporciona:

- ? **Estandarización**: Formato consistente reconocido por la industria
- ? **Información estructurada**: Campos predefinidos para describir errores
- ? **Extensibilidad**: Permite agregar campos personalizados
- ? **Compatibilidad**: Integración nativa con ASP.NET Core
- ? **Debugging mejorado**: Incluye información de rastreo

---

## Estructura de ProblemDetails

### Campos Estándar

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Error de regla de negocio",
  "status": 400,
  "detail": "El usuario emisor 'Juan Pérez' está inactivo y no puede crear solicitudes.",
  "instance": "/api/Solicitud",
  "traceId": "00-1234567890abcdef-1234567890abcdef-00"
}
```

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `type` | string | URI que identifica el tipo de problema |
| `title` | string | Resumen corto legible por humanos |
| `status` | int | Código de estado HTTP |
| `detail` | string | Explicación detallada específica de este error |
| `instance` | string | URI que identifica la instancia específica del problema |
| `traceId` | string | Identificador de rastreo para debugging |

### Campos Extendidos (Opcionales)

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Error de validación",
  "status": 400,
  "detail": "Los datos proporcionados no cumplen con las reglas de validación.",
  "instance": "/api/Solicitud",
  "traceId": "00-abc123...",
  "errors": {
    "IdEmisor": ["El ID del emisor es obligatorio"],
    "IdActivo": ["El ID del activo debe ser mayor a 0"]
  }
}
```

---

## Mapeo de Excepciones a ProblemDetails

### 1. ValidationException

**Excepción:**
```csharp
throw new ValidationException("Errores de validación", errors);
```

**ProblemDetails Generado:**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Error de validación",
  "status": 400,
  "detail": "Errores de validación",
  "instance": "/api/Solicitud",
  "traceId": "00-abc123...",
  "errors": {
    "Motivo": ["El motivo es obligatorio"]
  }
}
```

---

### 2. NotFoundException

**Excepción:**
```csharp
throw new NotFoundException("No se encontró el usuario con ID 5.");
```

**ProblemDetails Generado:**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "Recurso no encontrado",
  "status": 404,
  "detail": "No se encontró el usuario con ID 5.",
  "instance": "/api/Usuario/5",
  "traceId": "00-def456..."
}
```

---

### 3. BusinessException

**Excepción:**
```csharp
throw new BusinessException("El usuario está inactivo y no puede crear solicitudes.");
```

**ProblemDetails Generado:**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Error de regla de negocio",
  "status": 400,
  "detail": "El usuario está inactivo y no puede crear solicitudes.",
  "instance": "/api/Solicitud",
  "traceId": "00-ghi789..."
}
```

---

### 4. ArgumentNullException / ArgumentException

**Excepción:**
```csharp
throw new ArgumentNullException(nameof(solicitud), "La solicitud no puede ser nula.");
```

**ProblemDetails Generado:**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Argumento nulo o inválido",
  "status": 400,
  "detail": "La solicitud no puede ser nula. (Parameter 'solicitud')",
  "instance": "/api/Solicitud",
  "traceId": "00-jkl012...",
  "paramName": "solicitud"
}
```

---

### 5. DbUpdateException

**Excepción:**
```csharp
// Error de clave duplicada en base de datos
```

**ProblemDetails Generado:**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Error de base de datos",
  "status": 400,
  "detail": "Ya existe un registro con los mismos valores únicos.",
  "instance": "/api/Usuario",
  "traceId": "00-mno345..."
}
```

**Detección Inteligente de Errores SQL:**
- ? Clave duplicada (UNIQUE KEY)
- ? Violación de FK (FOREIGN KEY)
- ? Violación de PK (PRIMARY KEY)
- ? Restricción CHECK
- ? Error de eliminación con referencias

---

### 6. UnauthorizedAccessException

**Excepción:**
```csharp
throw new UnauthorizedAccessException();
```

**ProblemDetails Generado:**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "No autorizado",
  "status": 401,
  "detail": "No tiene permisos para realizar esta operación.",
  "instance": "/api/Activo/5",
  "traceId": "00-pqr678..."
}
```

---

### 7. Exception Genérica (500)

**Excepción:**
```csharp
throw new Exception("Error inesperado");
```

**ProblemDetails Generado:**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.6.1",
  "title": "Error interno del servidor",
  "status": 500,
  "detail": "Ocurrió un error inesperado. Por favor, contacte al administrador.",
  "instance": "/api/Solicitud",
  "traceId": "00-stu901...",
  "exceptionType": "NullReferenceException"
}
```

---

## Ventajas de ProblemDetails

### Antes (Respuesta Custom)
```json
{
  "error": "Error de negocio",
  "message": "El usuario está inactivo"
}
```

? **Problemas:**
- No estándar
- Sin información de rastreo
- No extensible
- Sin tipo de problema
- Difícil de documentar

### Ahora (ProblemDetails)
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Error de regla de negocio",
  "status": 400,
  "detail": "El usuario está inactivo",
  "instance": "/api/Solicitud",
  "traceId": "00-abc123..."
}
```

? **Ventajas:**
- Estándar RFC 7807
- Información de rastreo
- Extensible con campos custom
- Tipo de problema identificable
- Fácil de documentar
- Compatible con herramientas

---

## Content-Type

### Antes
```
Content-Type: application/json
```

### Ahora
```
Content-Type: application/problem+json
```

Este content-type permite a los clientes identificar automáticamente que la respuesta es un error estructurado según RFC 7807.

---

## Ejemplos Completos

### Ejemplo 1: Error de Validación con FluentValidation

**Request:**
```bash
POST /api/Solicitud
{
  "idEmisor": 0,
  "idReceptor": 0,
  "idActivo": 0,
  "tipo": "",
  "mensaje": ""
}
```

**Response (400 Bad Request):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Error de validación",
  "status": 400,
  "detail": "Uno o más errores de validación ocurrieron.",
  "instance": "/api/Solicitud",
  "traceId": "00-1234567890abcdef1234567890abcdef-1234567890abcdef-00",
  "errors": {
    "IdEmisor": [
      "El ID del emisor es obligatorio y debe ser mayor a 0."
    ],
    "IdReceptor": [
      "El ID del receptor es obligatorio y debe ser mayor a 0."
    ],
    "IdActivo": [
      "El ID del activo es obligatorio y debe ser mayor a 0."
    ],
    "Tipo": [
      "El tipo de solicitud es obligatorio."
    ]
  }
}
```

---

### Ejemplo 2: Recurso No Encontrado

**Request:**
```bash
GET /api/Usuario/claveFortia/EMP999
```

**Response (404 Not Found):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "Recurso no encontrado",
  "status": 404,
  "detail": "No se encontró ningún usuario con la clave Fortia 'EMP999'.",
  "instance": "/api/Usuario/claveFortia/EMP999",
  "traceId": "00-abcdef1234567890abcdef1234567890-abcdef1234567890-00"
}
```

---

### Ejemplo 3: Error de Regla de Negocio

**Request:**
```bash
POST /api/Solicitud
{
  "idEmisor": 5,
  "idReceptor": 2,
  "idActivo": 10,
  "tipo": "Transferencia",
  "mensaje": "Solicitud"
}
```

**Response (400 Bad Request):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Error de regla de negocio",
  "status": 400,
  "detail": "El usuario emisor 'Pedro Martínez López' está inactivo y no puede crear solicitudes.",
  "instance": "/api/Solicitud",
  "traceId": "00-fedcba0987654321fedcba0987654321-fedcba0987654321-00"
}
```

---

### Ejemplo 4: Error de Base de Datos

**Request:**
```bash
POST /api/Usuario
{
  "correo": "juan@example.com" // Correo duplicado
}
```

**Response (400 Bad Request):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Error de base de datos",
  "status": 400,
  "detail": "Ya existe un registro con los mismos valores únicos.",
  "instance": "/api/Usuario",
  "traceId": "00-1a2b3c4d5e6f7890-1a2b3c4d5e6f7890-00"
}
```

---

## Uso en Cliente (JavaScript/TypeScript)

### Detección de Errores

```typescript
async function crearSolicitud(solicitud: CreateSolicitudDto) {
  try {
    const response = await fetch('/api/Solicitud', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(solicitud)
    });

    if (!response.ok) {
      const problemDetails = await response.json();
      
      // Verificar si es ProblemDetails
      if (problemDetails.type && problemDetails.title) {
        console.error('Error:', problemDetails.title);
        console.error('Detalle:', problemDetails.detail);
        console.error('TraceId:', problemDetails.traceId);
        
        // Manejar errores de validación
        if (problemDetails.errors) {
          Object.keys(problemDetails.errors).forEach(field => {
            console.error(`${field}:`, problemDetails.errors[field]);
          });
        }
      }
      
      throw new Error(problemDetails.detail);
    }

    return await response.json();
  } catch (error) {
    console.error('Error al crear solicitud:', error);
    throw error;
  }
}
```

---

## Logging y Debugging

### TraceId para Correlación

El `traceId` incluido en cada ProblemDetails permite:

1. ? **Correlacionar logs**: Buscar en logs usando el traceId
2. ? **Debugging**: Identificar la request específica que falló
3. ? **Soporte**: El usuario puede reportar el traceId
4. ? **Monitoreo**: Rastrear errores en herramientas APM

### Ejemplo de Log

```
[Error] 2024-11-12 14:30:00 - TraceId: 00-abc123... - BusinessException: El usuario está inactivo
```

---

## ApiResponse para Respuestas Exitosas

Para respuestas exitosas, se mantiene `ApiResponse<T>` simplificado:

```csharp
return Ok(ApiResponse<SolicitudDto>.SuccessResponse(solicitud, "Solicitud creada correctamente"));
```

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Solicitud creada correctamente",
  "data": {
    "idSolicitud": 15,
    "idEmisor": 1,
    "nombreEmisor": "Juan Pérez",
    ...
  }
}
```

---

## Documentación en Swagger

ProblemDetails se documenta automáticamente en Swagger/OpenAPI:

```yaml
responses:
  '400':
    description: Bad Request
    content:
      application/problem+json:
        schema:
          $ref: '#/components/schemas/ProblemDetails'
  '404':
    description: Not Found
    content:
      application/problem+json:
        schema:
          $ref: '#/components/schemas/ProblemDetails'
```

---

## Componentes Modificados

### Middleware
- ? `ExceptionHandlingMiddleware.cs` - Refactorizado para usar ProblemDetails

### Response Classes
- ? `ApiResponse.cs` - Simplificado para solo respuestas exitosas

---

## Mejores Prácticas Implementadas

1. ? **Estándar RFC 7807**: Formato reconocido internacionalmente
2. ? **Content-Type apropiado**: `application/problem+json`
3. ? **TraceId incluido**: Para debugging y correlación
4. ? **Mensajes claros**: Descripciones útiles para desarrolladores
5. ? **Información estructurada**: Campos estándar + extensiones
6. ? **No exponer detalles internos**: En errores 500
7. ? **Parsing inteligente**: Errores de BD descriptivos
8. ? **Extensibilidad**: Campos custom cuando sea necesario

---

## Testing

### Ejemplo de Test

```csharp
[Fact]
public async Task CreateSolicitud_WithInactiveUser_ReturnsProblemDetails()
{
    // Arrange
    var solicitud = new CreateSolicitudDto { IdEmisor = 5 }; // Usuario inactivo

    // Act
    var response = await _client.PostAsJsonAsync("/api/Solicitud", solicitud);

    // Assert
    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    
    var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
    Assert.NotNull(problemDetails);
    Assert.Equal(400, problemDetails.Status);
    Assert.Equal("Error de regla de negocio", problemDetails.Title);
    Assert.Contains("inactivo", problemDetails.Detail);
    Assert.NotNull(problemDetails.Extensions["traceId"]);
}
```

---

## Estado Final

- ? **ProblemDetails implementado** según RFC 7807
- ? **Content-Type correcto**: `application/problem+json`
- ? **TraceId para debugging**
- ? **Extensiones personalizadas** (errors, paramName, etc.)
- ? **Parsing inteligente** de errores de BD
- ? **Compilación exitosa**
- ? **Estándar de la industria** ??

¡El manejo de errores ahora sigue el estándar RFC 7807 y es más profesional y consistente!
