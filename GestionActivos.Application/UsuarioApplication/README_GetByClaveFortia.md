# Endpoint: Obtener Usuario por Clave Fortia

## Descripción
Este endpoint permite obtener la información de un usuario junto con su centro de costo mediante su clave Fortia.

---

## URL
```
GET /api/Usuario/claveFortia/{claveFortia}
```

## Parámetros de URL

| Parámetro | Tipo | Descripción |
|-----------|------|-------------|
| `claveFortia` | string | Clave Fortia del usuario a consultar |

---

## Ejemplo de Request

```bash
curl -X GET "https://localhost:7000/api/Usuario/claveFortia/EMP001"
```

---

## Respuesta Exitosa (200 OK)

```json
{
  "idUsuario": 1,
  "nombreCompleto": "Juan Pérez García",
  "claveFortia": "EMP001",
  "idCentroCosto": 5,
  "ubicacion": "Ciudad de México",
  "razonSocial": "Empresa ABC S.A. de C.V.",
  "area": "Desarrollo de Software"
}
```

### Campos de Respuesta

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `idUsuario` | int | ID único del usuario |
| `nombreCompleto` | string | Nombre completo del usuario (Nombres + Apellido Paterno + Apellido Materno) |
| `claveFortia` | string | Clave Fortia del usuario |
| `idCentroCosto` | int? | ID del centro de costo (puede ser null) |
| `ubicacion` | string? | Ubicación del centro de costo (puede ser null) |
| `razonSocial` | string? | Razón social del centro de costo (puede ser null) |
| `area` | string? | Área del centro de costo (puede ser null) |

---

## Respuestas de Error

### 404 Not Found - Usuario No Encontrado
```json
{
  "message": "No se encontró ningún usuario con la clave Fortia 'EMP999'."
}
```

### 400 Bad Request - Clave Fortia Vacía
```json
{
  "message": "La clave Fortia no puede estar vacía."
}
```

---

## Ejemplos de Uso

### Ejemplo 1: Usuario con Centro de Costo

**Request:**
```bash
GET /api/Usuario/claveFortia/EMP001
```

**Response:**
```json
{
  "idUsuario": 1,
  "nombreCompleto": "María González López",
  "claveFortia": "EMP001",
  "idCentroCosto": 3,
  "ubicacion": "Guadalajara, Jalisco",
  "razonSocial": "Tecnología Innovadora S.A.",
  "area": "Recursos Humanos"
}
```

### Ejemplo 2: Usuario sin Centro de Costo

**Request:**
```bash
GET /api/Usuario/claveFortia/EMP002
```

**Response:**
```json
{
  "idUsuario": 2,
  "nombreCompleto": "Carlos Ramírez",
  "claveFortia": "EMP002",
  "idCentroCosto": null,
  "ubicacion": null,
  "razonSocial": null,
  "area": null
}
```

---

## Estructura Creada

### Capa de Dominio
- ? `IUsuarioRepository` - Agregado método `GetByClaveFortiaAsync`

### Capa de Aplicación
- ? `UsuarioCentroCostoDto` - DTO de respuesta con información del usuario y centro de costo
- ? `GetUsuarioByClaveFortiaQuery` - Query para obtener usuario por clave Fortia

### Capa de Infraestructura
- ? `UsuarioRepository` - Implementado método `GetByClaveFortiaAsync` con Include de CentroCosto

### Capa de API
- ? `UsuarioController` - Endpoint GET con ruta `/claveFortia/{claveFortia}`

---

## Notas Técnicas

1. **Include de CentroCosto**: 
   - El repositorio hace un `Include` de la relación `CentroCosto` para evitar lazy loading
   - Esto garantiza que toda la información del centro de costo esté disponible en una sola consulta

2. **Construcción del Nombre Completo**:
   - Se concatenan: `Nombres + ApellidoPaterno + ApellidoMaterno` (si existe)
   - El apellido materno es opcional y solo se agrega si no es null o vacío

3. **Validaciones**:
   - Se valida que la clave Fortia no esté vacía
   - Se lanza `NotFoundException` si no se encuentra el usuario

4. **Campos Opcionales**:
   - Todos los campos del centro de costo son opcionales (nullable)
   - Si el usuario no tiene centro de costo asignado, estos campos serán `null`

5. **Diferenciación de Rutas**:
   - Se agregó `:int` a la ruta `[HttpGet("{id:int}")]` para evitar conflictos
   - La ruta por clave Fortia es explícita: `/claveFortia/{claveFortia}`

---

## Casos de Uso

Este endpoint es útil para:

? **Búsqueda rápida de empleados**: Buscar un usuario por su clave corporativa
? **Validación de asignaciones**: Verificar a qué centro de costo pertenece un usuario antes de asignar activos
? **Formularios de transferencia**: Autocompletar información cuando se ingresa una clave Fortia
? **Auditorías**: Consultar información completa de un usuario para reportes
? **Integraciones**: Sincronizar información con otros sistemas usando la clave Fortia

---

## Arquitectura

- Patrón CQRS con MediatR
- Repository Pattern para acceso a datos
- Manejo centralizado de excepciones
- DTOs para separación de concerns
