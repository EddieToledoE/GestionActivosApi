# Endpoint: Obtener Usuario por Clave Fortia

## Descripción
Este endpoint permite obtener la información de un usuario junto con su centro de costo mediante su clave Fortia.

**Nota:** Este endpoint valida que el usuario esté activo. Si el usuario está inactivo, se retorna un error explícito.

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

### 400 Bad Request - Usuario Inactivo ??
```json
{
  "message": "El usuario con clave Fortia 'EMP001' (Juan Pérez García) está inactivo."
}
```

**Razón:** El usuario existe en la base de datos pero ha sido desactivado. Esto ayuda a distinguir entre:
- ? Usuario no encontrado (404)
- ?? Usuario encontrado pero inactivo (400)

### 400 Bad Request - Clave Fortia Vacía
```json
{
  "message": "La clave Fortia no puede estar vacía."
}
```

---

## Flujo de Validación

```
1. ¿Clave Fortia está vacía?
   ? Sí: ArgumentException (400)
   ?
2. ¿Usuario existe?
   ? No: NotFoundException (404)
   ?
3. ¿Usuario está activo?
   ? No: BusinessException (400) ??
   ?
4. ? Retornar información del usuario con centro de costo
```

---

## Ejemplos de Uso

### Ejemplo 1: Usuario Activo con Centro de Costo

**Request:**
```bash
GET /api/Usuario/claveFortia/EMP001
```

**Response (200 OK):**
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

---

### Ejemplo 2: Usuario Activo sin Centro de Costo

**Request:**
```bash
GET /api/Usuario/claveFortia/EMP002
```

**Response (200 OK):**
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

### Ejemplo 3: Usuario Inactivo ??

**Request:**
```bash
GET /api/Usuario/claveFortia/EMP003
```

**Response (400 Bad Request):**
```json
{
  "message": "El usuario con clave Fortia 'EMP003' (Pedro Martínez López) está inactivo."
}
```

**Razón:** El usuario existe pero está desactivado en el sistema.

---

### Ejemplo 4: Usuario No Encontrado

**Request:**
```bash
GET /api/Usuario/claveFortia/EMP999
```

**Response (404 Not Found):**
```json
{
  "message": "No se encontró ningún usuario con la clave Fortia 'EMP999'."
}
```

---

## Diferencias entre Errores

| Error | Código | Descripción | Acción Sugerida |
|-------|--------|-------------|-----------------|
| **No encontrado** | 404 | El usuario no existe en BD | Verificar clave Fortia |
| **Inactivo** | 400 | El usuario existe pero está inactivo | Reactivar usuario o usar otro |
| **Clave vacía** | 400 | No se proporcionó clave Fortia | Proporcionar clave válida |

---

## Casos de Uso

### 1. Validación en Formularios
**Uso:** Verificar que un usuario esté activo antes de asignarlo a un activo.

**Flujo:**
```javascript
async function validarUsuario(claveFortia) {
  try {
    const response = await fetch(`/api/Usuario/claveFortia/${claveFortia}`);
    if (response.ok) {
      const usuario = await response.json();
      // Usuario válido y activo
      return { valid: true, usuario };
    }
  } catch (error) {
    if (error.status === 400) {
      // Usuario inactivo
      return { valid: false, reason: 'Usuario inactivo' };
    } else if (error.status === 404) {
      // Usuario no encontrado
      return { valid: false, reason: 'Usuario no encontrado' };
    }
  }
}
```

---

### 2. Autocompletado Inteligente
**Uso:** Mostrar información del usuario al escribir su clave Fortia.

**UI Feedback:**
- ? Verde: Usuario encontrado y activo
- ?? Amarillo: Usuario encontrado pero inactivo
- ? Rojo: Usuario no encontrado

---

### 3. Auditoría de Accesos
**Uso:** Verificar si un usuario puede acceder al sistema.

**Respuesta:**
- `200 OK`: Usuario activo ? Permitir acceso
- `400 Bad Request`: Usuario inactivo ? Denegar acceso
- `404 Not Found`: Usuario no existe ? Denegar acceso

---

## Validaciones Implementadas

### ? 1. Clave Fortia No Vacía
```csharp
if (string.IsNullOrWhiteSpace(request.ClaveFortia))
{
    throw new ArgumentException("La clave Fortia no puede estar vacía.");
}
```

### ? 2. Usuario Existe
```csharp
if (usuario == null)
{
    throw new NotFoundException($"No se encontró ningún usuario...");
}
```

### ? 3. Usuario Activo ??
```csharp
if (!usuario.Activo)
{
    throw new BusinessException(
        $"El usuario con clave Fortia '{request.ClaveFortia}' ({usuario.Nombres}...) está inactivo.");
}
```

---

## Estructura Creada

### Capa de Dominio
- ? `IUsuarioRepository` - Agregado método `GetByClaveFortiaAsync`

### Capa de Aplicación
- ? `UsuarioCentroCostoDto` - DTO de respuesta con información del usuario y centro de costo
- ? `GetUsuarioByClaveFortiaQuery` - Query para obtener usuario por clave Fortia
- ? **Validación de usuario activo agregada** ??

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
   - Se lanza `BusinessException` si el usuario está inactivo ??

4. **Campos Opcionales**:
   - Todos los campos del centro de costo son opcionales (nullable)
   - Si el usuario no tiene centro de costo asignado, estos campos serán `null`

5. **Diferenciación de Rutas**:
   - Se agregó `:int` a la ruta `[HttpGet("{id:int}")]` para evitar conflictos
   - La ruta por clave Fortia es explícita: `/claveFortia/{claveFortia}`

6. **Validación Explícita de Estado** ??:
   - Proporciona feedback claro al usuario sobre por qué no puede usar ese usuario
   - Ayuda a distinguir entre "no existe" y "existe pero inactivo"

---

## Mensajes de Error Mejorados

### Antes ?
```
GET /api/Usuario/claveFortia/EMP003
? 200 OK (retorna usuario inactivo)
```
**Problema:** No es claro que el usuario está inactivo.

### Ahora ?
```
GET /api/Usuario/claveFortia/EMP003
? 400 Bad Request
{
  "message": "El usuario con clave Fortia 'EMP003' (Pedro Martínez) está inactivo."
}
```
**Ventaja:** Mensaje explícito y claro sobre el estado del usuario.

---

## Casos de Uso en UI

### Formulario de Asignación de Activos

```javascript
// Validar usuario antes de permitir asignación
document.getElementById('claveFortia').addEventListener('blur', async (e) => {
  const claveFortia = e.target.value;
  
  try {
    const response = await fetch(`/api/Usuario/claveFortia/${claveFortia}`);
    
    if (response.ok) {
      const usuario = await response.json();
      // Usuario válido y activo
      mostrarInfo(usuario);
      habilitarBotonAsignar();
    }
  } catch (error) {
    const data = await error.response.json();
    
    if (error.response.status === 400) {
      // Usuario inactivo
      mostrarAdvertencia(data.message);
      deshabilitarBotonAsignar();
    } else if (error.response.status === 404) {
      // Usuario no encontrado
      mostrarError(data.message);
      deshabilitarBotonAsignar();
    }
  }
});
```

---

## Testing Recomendado

### Pruebas Unitarias
1. ? Buscar usuario activo ? Retorna información
2. ? Buscar usuario inactivo ? Error 400
3. ? Buscar usuario no existente ? Error 404
4. ? Buscar con clave vacía ? Error 400

### Pruebas de Integración
1. ? Flujo completo con usuario activo
2. ? Flujo completo con usuario inactivo
3. ? Verificar mensaje de error incluye nombre del usuario

---

## Arquitectura

- ? Patrón CQRS con MediatR
- ? Repository Pattern para acceso a datos
- ? Manejo centralizado de excepciones
- ? DTOs para separación de concerns
- ? Validaciones de negocio en la capa de aplicación
- ? Mensajes de error descriptivos y útiles ??

---

## Casos de Uso

Este endpoint es útil para:

? **Búsqueda rápida de empleados**: Buscar un usuario por su clave corporativa
? **Validación de asignaciones**: Verificar que el usuario esté activo antes de asignar activos
? **Formularios de transferencia**: Autocompletar información cuando se ingresa una clave Fortia
? **Auditorías**: Consultar información completa de un usuario para reportes
? **Integraciones**: Sincronizar información con otros sistemas usando la clave Fortia
? **Validación de estado**: Distinguir entre usuarios inexistentes e inactivos ??

---

## Comparación: Antes vs Ahora

| Aspecto | Antes | Ahora |
|---------|-------|-------|
| Usuario inactivo | Retorna 200 OK con datos | ? Error 400 explícito |
| Mensaje de error | Sin contexto | ? Incluye nombre del usuario |
| Feedback al usuario | Poco claro | ? Muy claro y accionable |
| Validación temprana | No | ? Sí, evita errores posteriores |

---

## Estado Final

- ? **Validación de usuario activo agregada**
- ? **Mensajes de error descriptivos**
- ? **Compilación exitosa**
- ? **Mejor experiencia de usuario**
- ? **Sistema más robusto** ??

¡El endpoint ahora valida explícitamente si el usuario está inactivo y proporciona un mensaje claro al respecto!
